using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents a 'ConcreteClass' in the Template Method GoF pattern (variant B).
    ///
    /// Domain role: rental-contract document — legally-formatted, with full
    /// T&amp;C and signature lines. ASCII art and visual formatting have been
    /// removed; in the web layer this will be a Razor partial view.
    /// </summary>
    public class ContractDocument : DocumentRenderer
    {
        public ContractDocument(ILogger<ContractDocument> logger) : base(logger) { }

        public override void RenderHeader()
        {
            Logger.LogDebug("[Pattern: Template Method] ContractDocument.RenderHeader — contract header rendered");
        }

        public override void RenderFooter()
        {
            Logger.LogDebug("[Pattern: Template Method] ContractDocument.RenderFooter — T&C and signature block rendered");
        }
    }
}
