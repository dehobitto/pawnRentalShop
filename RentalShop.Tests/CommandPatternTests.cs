using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using RentalShop.Application.Commands;

namespace RentalShop.Tests
{
    /// <summary>
    /// Unit tests for the <b>Command GoF pattern</b> implementation.
    ///
    /// Covers <see cref="RentItemCommand"/>, <see cref="ReturnItemCommand"/>,
    /// and the <see cref="CashierConsole"/> Invoker.  No database is needed —
    /// commands encapsulate behaviour; the Receiver (<see cref="CashierTerminal"/>)
    /// is a pure in-memory object.  A private <see cref="TrackingCommand"/> spy
    /// lets us assert Execute / Undo call sequences without relying on log output.
    /// </summary>
    public sealed class CommandPatternTests
    {
        private static CashierTerminal BuildTerminal()
            => new(NullLogger<CashierTerminal>.Instance);

        // ── RentItemCommand ─────────────────────────────────────────────────

        [Fact]
        public async Task RentItemCommand_ExecuteAsync_CompletesSuccessfully()
        {
            var cmd = new RentItemCommand(
                BuildTerminal(), "TOOL-001", 3,
                NullLogger<RentItemCommand>.Instance);

            // Should not throw
            await cmd.ExecuteAsync();
        }

        [Fact]
        public async Task RentItemCommand_UndoAsync_AfterExecute_CompletesSuccessfully()
        {
            var cmd = new RentItemCommand(
                BuildTerminal(), "TOOL-001", 3,
                NullLogger<RentItemCommand>.Instance);

            await cmd.ExecuteAsync();
            await cmd.UndoAsync(); // should not throw
        }

        // ── ReturnItemCommand ───────────────────────────────────────────────

        [Fact]
        public async Task ReturnItemCommand_ExecuteAsync_CompletesSuccessfully()
        {
            var cmd = new ReturnItemCommand(
                BuildTerminal(), "TOOL-001",
                NullLogger<ReturnItemCommand>.Instance);

            await cmd.ExecuteAsync();
        }

        [Fact]
        public async Task ReturnItemCommand_UndoAsync_AfterExecute_CompletesSuccessfully()
        {
            var cmd = new ReturnItemCommand(
                BuildTerminal(), "TOOL-001",
                NullLogger<ReturnItemCommand>.Instance);

            await cmd.ExecuteAsync();
            await cmd.UndoAsync();
        }

        // ── CashierConsole (Invoker) ────────────────────────────────────────

        [Fact]
        public async Task CashierConsole_ExecuteStagedThenUndo_CallsUndoOnExecutedCommand()
        {
            var console = new CashierConsole(
                NullLogger<CashierConsole>.Instance);
            var spy = new TrackingCommand(BuildTerminal());

            console.Stage(spy);
            await console.ExecuteStagedAsync();
            await console.UndoLastAsync();

            Assert.True(spy.ExecuteCalled, "ExecuteAsync should have been invoked");
            Assert.True(spy.UndoCalled,    "UndoAsync should have been invoked after Undo");
        }

        [Fact]
        public async Task CashierConsole_ExecuteStaged_WithNoCommand_DoesNothing()
        {
            var console = new CashierConsole(
                NullLogger<CashierConsole>.Instance);

            // No command staged — must not throw
            await console.ExecuteStagedAsync();
        }

        [Fact]
        public async Task CashierConsole_StagedCommand_ClearedAfterExecute_PreventingDoubleExecution()
        {
            var console = new CashierConsole(
                NullLogger<CashierConsole>.Instance);
            var spy = new TrackingCommand(BuildTerminal());

            console.Stage(spy);
            await console.ExecuteStagedAsync(); // executes + clears staged
            spy.Reset();
            await console.ExecuteStagedAsync(); // nothing staged → should be no-op

            Assert.False(spy.ExecuteCalled, "Second ExecuteStaged without re-staging must not re-fire the command");
        }

        // ── Spy ─────────────────────────────────────────────────────────────

        private sealed class TrackingCommand : CashierCommand
        {
            public bool ExecuteCalled { get; private set; }
            public bool UndoCalled   { get; private set; }

            public TrackingCommand(CashierTerminal terminal) : base(terminal) { }

            public void Reset() { ExecuteCalled = false; UndoCalled = false; }

            public override Task ExecuteAsync(CancellationToken ct = default)
            {
                ExecuteCalled = true;
                return Task.CompletedTask;
            }

            public override Task UndoAsync(CancellationToken ct = default)
            {
                UndoCalled = true;
                return Task.CompletedTask;
            }
        }
    }
}
