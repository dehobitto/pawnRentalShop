using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents a 'ConcreteClass' in the Template Method GoF pattern (variant B).
    ///
    /// Domain role: rental-contract document — long, legally-formatted, with
    /// full T&amp;C and signature lines.
    /// </summary>
    public class ContractDocument : DocumentRenderer
    {
        public ContractDocument(ILogger<ContractDocument> logger) : base(logger) { }

        public override void RenderHeader()
        {
            Logger.LogInformation("╔══ RENTAL CONTRACT #C-2026-0001 ══╗");
            Logger.LogInformation("║ Lessor: RentalShop Inc.          ║");
            Logger.LogInformation("║ Lessee: <customer info here>     ║");
            Logger.LogInformation("╚══════════════════════════════════╝");
        }

        public override void RenderFooter()
        {
            Logger.LogInformation("Terms & conditions: <T&C body>");
            Logger.LogInformation("Lessee signature: __________________");
            Logger.LogInformation("Lessor signature: __________________");
        }
    }
}
