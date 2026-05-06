using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents a 'ConcreteClass' in the Template Method GoF pattern (variant A).
    ///
    /// Domain role: receipt document — short, customer-facing, focused on
    /// totals and payment confirmation. ASCII art and visual formatting have
    /// been removed; in the web layer this will be a Razor partial view.
    /// </summary>
    public class ReceiptDocument : DocumentRenderer
    {
        public ReceiptDocument(ILogger<ReceiptDocument> logger) : base(logger) { }

        public override void RenderHeader()
        {
            Logger.LogDebug("[Pattern: Template Method] ReceiptDocument.RenderHeader — receipt header rendered");
        }

        public override void RenderFooter()
        {
            Logger.LogDebug("[Pattern: Template Method] ReceiptDocument.RenderFooter — receipt footer rendered");
        }
    }
}
