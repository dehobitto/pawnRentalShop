using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Commands
{
    /// <summary>
    /// Represents the 'Receiver' in the Command GoF pattern — the object that
    /// actually performs the work commanded.
    ///
    /// Domain role: cashier terminal / domain service that commits the
    /// transaction. Holds the real domain logic; commands delegate to it.
    /// </summary>
    public class CashierTerminal
    {
        private readonly ILogger<CashierTerminal> _logger;

        public CashierTerminal(ILogger<CashierTerminal> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Performs the action requested by the active command. In a real
        /// system this would persist the transaction to the cashier's ledger.
        /// </summary>
        public Task CommitAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("Cashier-terminal action committed");
            return Task.CompletedTask;
        }
    }
}
