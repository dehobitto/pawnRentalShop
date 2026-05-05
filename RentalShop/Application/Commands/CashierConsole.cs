using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Commands
{
    /// <summary>
    /// Represents the 'Invoker' in the Command GoF pattern — holds a
    /// reference to a command and triggers it.
    ///
    /// Domain role: cashier console that stages and runs cashier commands
    /// while remembering them for undo. Decouples the UI button-press from
    /// the actual operation, which is what makes undo / redo / replay
    /// possible.
    /// </summary>
    public class CashierConsole
    {
        private CashierCommand? _staged;
        private readonly Stack<CashierCommand> _history = new();
        private readonly ILogger<CashierConsole> _logger;

        public CashierConsole(ILogger<CashierConsole> logger)
        {
            _logger = logger;
        }

        /// <summary>Stage the next cashier action.</summary>
        public void Stage(CashierCommand command) => _staged = command;

        /// <summary>Fire the staged action and remember it for a possible undo.</summary>
        public async Task ExecuteStagedAsync(CancellationToken ct = default)
        {
            if (_staged is null) return;
            var command = _staged;
            _staged = null;              // clear before execute — prevents double-fire on re-entry
            await command.ExecuteAsync(ct);
            _history.Push(command);
        }

        /// <summary>Roll back the most recently executed action ("void last transaction").</summary>
        public async Task UndoLastAsync(CancellationToken ct = default)
        {
            if (_history.Count == 0)
            {
                _logger.LogWarning("Nothing to undo");
                return;
            }
            await _history.Pop().UndoAsync(ct);
        }
    }
}
