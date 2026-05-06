using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Commands
{
    /// <summary>
    /// Represents a 'ConcreteCommand' in the Command GoF pattern (variant B).
    ///
    /// Domain role: "Return" cashier action. Encapsulates everything needed
    /// to take an item back so the operation can be queued or undone (e.g. if
    /// the item turns out to be damaged on inspection).
    /// </summary>
    public class ReturnItemCommand : CashierCommand
    {
        private readonly string _sku;
        private readonly ILogger<ReturnItemCommand> _logger;

        public ReturnItemCommand(CashierTerminal terminal, string sku, ILogger<ReturnItemCommand> logger)
            : base(terminal)
        {
            ArgumentNullException.ThrowIfNull(terminal);
            ArgumentNullException.ThrowIfNull(sku);
            ArgumentNullException.ThrowIfNull(logger);
            _sku    = sku;
            _logger = logger;
        }

        public override async Task ExecuteAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Command] ReturnItemCommand.ExecuteAsync dispatching for {Sku}", _sku);
            await Terminal.CommitAsync(ct);
            _logger.LogInformation("Item {Sku} returned by customer", _sku);
        }

        public override Task UndoAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Command] ReturnItemCommand.UndoAsync invoked for {Sku}", _sku);
            _logger.LogInformation("Undo: Item {Sku} re-issued to customer (return reversed)", _sku);
            return Task.CompletedTask;
        }
    }
}
