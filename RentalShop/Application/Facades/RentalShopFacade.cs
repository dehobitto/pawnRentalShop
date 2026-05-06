using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RentalShop.Application.Factories;
using RentalShop.Application.Services;
using RentalShop.Domain.Entities;
using RentalShop.Models;

namespace RentalShop.Application.Facades
{
    /// <summary>
    /// Represents the 'Facade' in the Facade GoF pattern — exposes a unified,
    /// high-level interface that hides the complexity of several subsystems.
    ///
    /// Domain role: single entry point the UI talks to. Provides high-level
    /// rental-shop use-cases and orchestrates the inventory, billing, payment,
    /// document, and factory subsystems. All subsystems are injected so the
    /// Facade is a proper DI citizen and each subsystem remains independently
    /// testable.
    /// </summary>
    public class RentalShopFacade
    {
        private readonly InventoryService  _inventory;
        private readonly BillingService    _billing;
        private readonly PaymentService    _payment;
        private readonly DocumentService   _document;
        private readonly DocumentRenderers _renderers;
        private readonly ToolFactory       _toolFactory;
        private readonly GearFactory       _gearFactory;
        private readonly ILogger<RentalShopFacade> _logger;

        public RentalShopFacade(
            InventoryService  inventory,
            BillingService    billing,
            PaymentService    payment,
            DocumentService   document,
            DocumentRenderers renderers,
            ToolFactory       toolFactory,
            GearFactory       gearFactory,
            ILogger<RentalShopFacade> logger)
        {
            _inventory   = inventory;
            _billing     = billing;
            _payment     = payment;
            _document    = document;
            _renderers   = renderers;
            _toolFactory = toolFactory;
            _gearFactory = gearFactory;
            _logger      = logger;
        }

        /// <summary>Returns the full rental catalog for display in the UI.</summary>
        public Task<IReadOnlyList<RentalItem>> GetCatalogAsync(CancellationToken ct = default)
            => _inventory.GetAllItemsAsync(ct);

        /// <summary>
        /// Pure read-only use-case. Returns a single item without modifying any state.
        /// The Proxy pattern (via <c>ICatalogRepository</c>) ensures that repeated
        /// calls for the same SKU are served from the in-process cache (cache HIT),
        /// producing zero database round-trips after the first request.
        /// Returns <c>null</c> when the SKU is not found.
        /// </summary>
        public Task<RentalItem?> GetItemDetailsAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Facade] RentalShopFacade.GetItemDetailsAsync for {Sku}", sku);
            return _inventory.GetItemDetailsAsync(sku, ct);
        }

        /// <summary>
        /// "Process rental" use-case. Advances the item's lifecycle state
        /// (Available → Rented via the State pattern), prices it, charges a
        /// deposit, and emits the rental contract.
        ///
        /// Returns <c>false</c> when:
        /// <list type="bullet">
        ///   <item>The SKU is unknown.</item>
        ///   <item>The current state does not permit renting (e.g. already rented).</item>
        ///   <item>A concurrent request rented the same item first
        ///         (<see cref="DbUpdateConcurrencyException"/>).</item>
        /// </list>
        /// </summary>
        public async Task<bool> ProcessRentalAsync(string sku, int days, CancellationToken ct = default)
        {
            _logger.LogInformation("Rental started: Item {Sku} for {Days} day(s)", sku, days);
            RentalItem? item;
            try
            {
                item = await _inventory.RentItemAsync(sku, days, ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Two users read the same Available item from the cache and submitted
                // simultaneously. The first writer bumped Version; the second writer's
                // WHERE Version=<stale> matched 0 rows → EF Core raised this exception.
                // The Proxy already invalidated the cache before the DB call, so the
                // next request will re-fetch the current (Rented) state from the DB.
                _logger.LogWarning(ex,
                    "Concurrency conflict detected: User attempted to rent {Sku} but it was modified by another transaction.",
                    sku);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Rental rejected for {Sku}: {Reason}", sku, ex.Message);
                return false;
            }
            if (item is null) return false;

            var total = _billing.CalculateTotal(item.BasePricePerDay, days);
            await _payment.CaptureAsync(total, ct);
            _document.UseRenderer(_renderers.Contract);
            await _document.GenerateAsync(ct);
            _logger.LogInformation("Rental completed: Item {Sku}, charged €{Total}", sku, total);
            return true;
        }

        /// <summary>
        /// "Process return" use-case. Advances the item's lifecycle state
        /// (Rented → Available via the State pattern), settles the balance,
        /// and emits the receipt.
        ///
        /// Returns <c>false</c> when the SKU is unknown, the current state does
        /// not permit returning, or a concurrency conflict is detected.
        /// </summary>
        public async Task<bool> ProcessReturnAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogInformation("Return started: Item {Sku}", sku);
            RentalItem? item;
            try
            {
                item = await _inventory.ReturnItemAsync(sku, ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex,
                    "Concurrency conflict detected: User attempted to return {Sku} but it was modified by another transaction.",
                    sku);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Return rejected for {Sku}: {Reason}", sku, ex.Message);
                return false;
            }
            if (item is null) return false;

            await _payment.CaptureAsync(decimal.Zero, ct);
            _document.UseRenderer(_renderers.Receipt);
            await _document.GenerateAsync(ct);
            _logger.LogInformation("Return completed: Item {Sku}", sku);
            return true;
        }

        /// <summary>
        /// "Delete catalog item" use-case. Only Available items may be deleted.
        /// Returns <c>false</c> when the SKU is unknown or the item is currently
        /// Rented or UnderRepair.
        /// </summary>
        public Task<bool> DeleteCatalogItemAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogInformation("Delete requested: Item {Sku}", sku);
            return _inventory.DeleteItemAsync(sku, ct);
        }

        /// <summary>
        /// "Add catalog item" use-case. Uses the correct concrete
        /// <see cref="ItemFactory"/> (Factory Method pattern) based on category.
        /// SKU is auto-generated by the factory — callers supply only name,
        /// price, and category.
        /// </summary>
        public async Task<bool> CreateCatalogItemAsync(
            string name, decimal price, ItemCategory category,
            CancellationToken ct = default)
        {
            ItemFactory factory = category == ItemCategory.Gear ? _gearFactory : _toolFactory;
            var item = factory.Create(name, price);
            await _inventory.AddItemAsync(item, ct);
            _logger.LogInformation("[Pattern: Factory Method] {Category} item {Sku} added to catalog",
                category, item.Sku);
            return true;
        }
    }
}
