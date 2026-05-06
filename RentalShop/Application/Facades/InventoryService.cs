using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RentalShop.Domain.Entities;
using RentalShop.Domain.States;
using RentalShop.Infrastructure.Repositories;

namespace RentalShop.Application.Facades
{
    /// <summary>
    /// Represents a 'SubSystem' in the Facade GoF pattern (subsystem #1).
    ///
    /// Domain role: inventory subsystem — checks item availability via the
    /// caching <see cref="ICatalogRepository"/> and manages state transitions.
    /// </summary>
    public class InventoryService
    {
        private readonly ICatalogRepository _catalog;
        private readonly ILogger<InventoryService> _logger;
        private readonly ILogger<ItemLifecycle> _lifecycleLogger;

        public InventoryService(
            ICatalogRepository catalog,
            ILogger<InventoryService> logger,
            ILogger<ItemLifecycle> lifecycleLogger)
        {
            _catalog         = catalog;
            _logger          = logger;
            _lifecycleLogger = lifecycleLogger;
        }

        /// <summary>Look the SKU up in the catalog without changing state.</summary>
        public async Task<RentalItem?> ReserveAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Facade] InventoryService.ReserveAsync for {Sku}", sku);
            var item = await _catalog.FindAsync(sku, ct);
            if (item is null) _logger.LogWarning("Item {Sku} not found in catalog", sku);
            else _logger.LogInformation("Item {Sku} ({Name}) reserved from catalog", sku, item.Name);
            return item;
        }

        /// <summary>Returns <c>true</c> if the SKU already exists in the catalog.</summary>
        public async Task<bool> ExistsAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Facade] InventoryService.ExistsAsync for {Sku}", sku);
            return await _catalog.FindAsync(sku, ct) is not null;
        }

        /// <summary>
        /// Finds the item and advances its lifecycle to <see cref="RentedState"/>
        /// via the State pattern. Sets <see cref="RentalItem.ExpectedReturnDate"/>
        /// to <c>UtcNow + <paramref name="days"/></c> before persisting.
        /// Returns <c>null</c> if the SKU is not found.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown by the active state when renting is not permitted
        /// (e.g. item already rented or under repair).
        /// </exception>
        public async Task<RentalItem?> RentItemAsync(string sku, int days, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Facade] InventoryService.RentItemAsync for {Sku} ({Days}d)", sku, days);
            var item = await _catalog.FindAsync(sku, ct);
            if (item is null)
            {
                _logger.LogWarning("Item {Sku} not found — rent aborted", sku);
                return null;
            }

            var lifecycle = new ItemLifecycle(item.CurrentState, _lifecycleLogger);
            lifecycle.Rent();
            item.CurrentState       = lifecycle.CurrentState;
            item.ExpectedReturnDate = DateTime.UtcNow.AddDays(days);

            await _catalog.CommitStateAsync(item, ct);
            _logger.LogInformation("Item {Sku} state → {State}, due back {Due:yyyy-MM-dd}",
                sku, item.CurrentState.GetType().Name, item.ExpectedReturnDate);
            return item;
        }

        /// <summary>
        /// Finds the item and advances its lifecycle to <see cref="AvailableState"/>
        /// via the State pattern. Bumps <see cref="RentalItem.Version"/> before
        /// persisting so EF Core's optimistic concurrency check can detect a
        /// simultaneous write. Returns <c>null</c> if the SKU is not found.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown by the active state when returning is not permitted
        /// (e.g. item not currently rented or under repair).
        /// </exception>
        public async Task<RentalItem?> ReturnItemAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Facade] InventoryService.ReturnItemAsync for {Sku}", sku);
            var item = await _catalog.FindAsync(sku, ct);
            if (item is null)
            {
                _logger.LogWarning("Item {Sku} not found — return aborted", sku);
                return null;
            }

            var lifecycle = new ItemLifecycle(item.CurrentState, _lifecycleLogger);
            lifecycle.Return();
            item.CurrentState       = lifecycle.CurrentState;
            item.ExpectedReturnDate = null;

            await _catalog.CommitStateAsync(item, ct);
            _logger.LogInformation("Item {Sku} state → {State}", sku, item.CurrentState.GetType().Name);
            return item;
        }

        /// <summary>
        /// Pure read-only lookup — delegates directly to <see cref="ICatalogRepository.FindAsync"/>.
        /// On first call for a SKU the Proxy fetches from the database and populates the cache;
        /// on subsequent calls the Proxy returns the cached entry (cache HIT) without touching the DB.
        /// </summary>
        public async Task<RentalItem?> GetItemDetailsAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Facade] InventoryService.GetItemDetailsAsync for {Sku}", sku);
            var item = await _catalog.FindAsync(sku, ct);
            if (item is null) _logger.LogWarning("Item {Sku} not found in catalog", sku);
            else _logger.LogInformation("[Pattern: Proxy] Details fetched for {Sku} ({Name}) — may be a cache HIT", sku, item.Name);
            return item;
        }

        /// <summary>Returns all items in the catalog for display purposes.</summary>
        public Task<IReadOnlyList<RentalItem>> GetAllItemsAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Facade] InventoryService.GetAllItemsAsync");
            return _catalog.GetAllAsync(ct);
        }

        /// <summary>Persists a newly created catalog item.</summary>
        public Task AddItemAsync(RentalItem item, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Facade] InventoryService.AddItemAsync for {Sku}", item.Sku);
            return _catalog.AddAsync(item, ct);
        }

        /// <summary>
        /// Removes a catalog item permanently. Only <see cref="AvailableState"/>
        /// items may be deleted — rented or under-repair items are refused so the
        /// system stays consistent with any open transaction records.
        /// Returns <c>false</c> when the SKU is not found or the item is not Available.
        /// </summary>
        public async Task<bool> DeleteItemAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Facade] InventoryService.DeleteItemAsync for {Sku}", sku);
            var item = await _catalog.FindAsync(sku, ct);
            if (item is null)
            {
                _logger.LogWarning("Item {Sku} not found — delete aborted", sku);
                return false;
            }
            if (item.CurrentState is not AvailableState)
            {
                _logger.LogWarning("Item {Sku} is {State} — only Available items can be deleted",
                    sku, item.CurrentState.GetType().Name);
                return false;
            }
            await _catalog.DeleteAsync(sku, ct);
            _logger.LogInformation("Item {Sku} deleted from catalog", sku);
            return true;
        }
    }
}
