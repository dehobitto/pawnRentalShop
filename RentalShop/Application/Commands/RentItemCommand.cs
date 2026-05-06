using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Commands
{
    /// <summary>
    /// Represents a 'ConcreteCommand' in the Command GoF pattern (variant A).
    ///
    /// Domain role: "Rent" cashier action. Encapsulates everything needed to
    /// hand an item to a customer — including the rental duration — so the
    /// operation can be queued, logged, or undone.
    /// </summary>
    public class RentItemCommand : CashierCommand
    {
        private readonly string _sku;
        private readonly int    _durationInDays;
        private readonly ILogger<RentItemCommand> _logger;

        public RentItemCommand(
            CashierTerminal terminal,
            string sku,
            int durationInDays,
            ILogger<RentItemCommand> logger)
            : base(terminal)
        {
            ArgumentNullException.ThrowIfNull(terminal);
            ArgumentNullException.ThrowIfNull(sku);
            ArgumentNullException.ThrowIfNull(logger);
            _sku            = sku;
            _durationInDays = durationInDays;
            _logger         = logger;
        }

        public override async Task ExecuteAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Command] RentItemCommand.ExecuteAsync dispatching for {Sku} ({Days}d)",
                _sku, _durationInDays);
            await Terminal.CommitAsync(ct);
            _logger.LogInformation("Item {Sku} issued to customer for {Days} day(s)", _sku, _durationInDays);
        }

        public override Task UndoAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Command] RentItemCommand.UndoAsync invoked for {Sku}", _sku);
            _logger.LogInformation("Undo: Item {Sku} returned to shelf (rent reversed)", _sku);
            return Task.CompletedTask;
        }
    }
}
