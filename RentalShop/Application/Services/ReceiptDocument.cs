using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents a 'ConcreteClass' in the Template Method GoF pattern (variant A).
    ///
    /// Domain role: receipt document — short, customer-facing, focused on
    /// totals and payment.
    /// </summary>
    public class ReceiptDocument : DocumentRenderer
    {
        public ReceiptDocument(ILogger<ReceiptDocument> logger) : base(logger) { }

        public override void RenderHeader()
        {
            Logger.LogInformation("╭── RECEIPT #R-2026-0001 ──╮");
            Logger.LogInformation("│ RentalShop Inc.          │");
            Logger.LogInformation("╰──────────────────────────╯");
        }

        public override void RenderFooter()
        {
            Logger.LogInformation("Thank you — please come again.");
        }
    }
}
