using System.Threading;
using System.Threading.Tasks;

namespace RentalShop.Application.Commands
{
    /// <summary>
    /// Represents the abstract 'Command' in the Command GoF pattern —
    /// declares the execute interface and (here) an undo hook.
    ///
    /// Domain role: encapsulates a single cashier action (rent, return) as
    /// an object so it can be queued, logged, undone, or replayed. Holds a
    /// reference to the <see cref="CashierTerminal"/> that does the actual
    /// work.
    /// </summary>
    public abstract class CashierCommand
    {
        /// <summary>
        /// The object that knows *how* to perform the action. Commands stay
        /// behaviour-free and dispatch to the terminal.
        /// </summary>
        protected readonly CashierTerminal Terminal;

        protected CashierCommand(CashierTerminal terminal)
        {
            Terminal = terminal;
        }

        /// <summary>Run this cashier action asynchronously.</summary>
        public abstract Task ExecuteAsync(CancellationToken ct = default);

        /// <summary>
        /// Reverse this cashier action — used to roll back the most recent
        /// operation. Default no-op for commands that aren't meaningfully
        /// undoable. NOTE: Undo is an extension over the canonical example.
        /// </summary>
        public virtual Task UndoAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}
