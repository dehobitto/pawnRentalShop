using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RentalShop.Application.Services;

namespace RentalShop.Application.Facades
{
    /// <summary>
    /// Represents the 'Facade' in the Facade GoF pattern — exposes a unified,
    /// high-level interface that hides the complexity of several subsystems.
    ///
    /// Domain role: single entry point the UI talks to. Provides high-level
    /// rental-shop use-cases ("process a rental", "process a return") and
    /// orchestrates the inventory, billing, payment, and document subsystems
    /// behind the scenes. All subsystems are injected so the Facade is a
    /// proper DI citizen and each subsystem remains independently testable.
    /// </summary>
    public class RentalShopFacade
    {
        private readonly InventoryService _inventory;
        private readonly BillingService _billing;
        private readonly PaymentService _payment;
        private readonly DocumentService _document;
        private readonly DocumentRenderer _contractRenderer;
        private readonly DocumentRenderer _receiptRenderer;
        private readonly ILogger<RentalShopFacade> _logger;

        public RentalShopFacade(
            InventoryService inventory,
            BillingService billing,
            PaymentService payment,
            DocumentService document,
            DocumentRenderer contractRenderer,
            DocumentRenderer receiptRenderer,
            ILogger<RentalShopFacade> logger)
        {
            _inventory = inventory;
            _billing = billing;
            _payment = payment;
            _document = document;
            _contractRenderer = contractRenderer;
            _receiptRenderer = receiptRenderer;
            _logger = logger;
        }

        /// <summary>
        /// "Process rental" use-case — reserve the item, price it, charge the
        /// deposit, emit the rental contract.
        /// </summary>
        public async Task ProcessRentalAsync(string sku, int days, CancellationToken ct = default)
        {
            _logger.LogInformation("ProcessRental: {Sku} for {Days}d", sku, days);
            var item = await _inventory.ReserveAsync(sku, ct);
            if (item is null) return;
            var total = _billing.CalculateTotal(item.BasePricePerDay, days);
            await _payment.CaptureAsync(total, ct);
            _document.UseRenderer(_contractRenderer);
            await _document.GenerateAsync(ct);
        }

        /// <summary>
        /// "Process return" use-case — mark the item available, settle the
        /// balance, emit the receipt. (Waitlist notification happens in the
        /// Observer layer.)
        /// </summary>
        public async Task ProcessReturnAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogInformation("ProcessReturn: {Sku}", sku);
            var item = await _inventory.ReserveAsync(sku, ct);
            if (item is null) return;
            await _payment.CaptureAsync(0m, ct);
            _document.UseRenderer(_receiptRenderer);
            await _document.GenerateAsync(ct);
        }
    }
}
