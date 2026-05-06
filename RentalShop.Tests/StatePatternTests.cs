using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using RentalShop.Domain.Entities;
using RentalShop.Domain.States;
using RentalShop.Tests.Helpers;

namespace RentalShop.Tests
{
    /// <summary>
    /// Integration tests for the <b>State GoF pattern</b> persistence layer.
    ///
    /// Each test verifies that <see cref="IItemState"/> concrete classes are
    /// round-tripped correctly through the EF Core
    /// <c>ValueConverter&lt;IItemState, string&gt;</c> defined in
    /// <c>RentalDbContext</c>: the state class name is written to
    /// <c>CurrentStateName</c>, and the correct concrete type is rehydrated on
    /// load from a separate <see cref="Microsoft.EntityFrameworkCore.DbContext"/>
    /// instance (simulating a real request boundary).
    /// </summary>
    public sealed class StatePatternTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly RentalShop.Infrastructure.Persistence.RentalDbContext _writeCtx;

        public StatePatternTests()
        {
            (_connection, _writeCtx) = DbContextFactory.Create();
        }

        // ── AvailableState ──────────────────────────────────────────────────

        [Fact]
        public async Task AvailableState_PersistedAsStringAndRehydrated()
        {
            _writeCtx.RentalItems.Add(new ToolItem("T-AVL", "Test Tool", 10m)
                { CurrentState = new AvailableState() });
            await _writeCtx.SaveChangesAsync();

            await using var fresh = DbContextFactory.CreateFresh(_connection);
            var item = await fresh.RentalItems.FindAsync("T-AVL");

            Assert.NotNull(item);
            Assert.IsType<AvailableState>(item.CurrentState);
        }

        // ── RentedState ─────────────────────────────────────────────────────

        [Fact]
        public async Task RentedState_PersistedAsStringAndRehydrated()
        {
            _writeCtx.RentalItems.Add(new ToolItem("T-RNT", "Test Tool", 10m)
                { CurrentState = new RentedState() });
            await _writeCtx.SaveChangesAsync();

            await using var fresh = DbContextFactory.CreateFresh(_connection);
            var item = await fresh.RentalItems.FindAsync("T-RNT");

            Assert.NotNull(item);
            Assert.IsType<RentedState>(item.CurrentState);
        }

        // ── UnderRepairState ────────────────────────────────────────────────

        [Fact]
        public async Task UnderRepairState_PersistedAsStringAndRehydrated()
        {
            _writeCtx.RentalItems.Add(new ToolItem("T-REP", "Test Tool", 10m)
                { CurrentState = new UnderRepairState() });
            await _writeCtx.SaveChangesAsync();

            await using var fresh = DbContextFactory.CreateFresh(_connection);
            var item = await fresh.RentalItems.FindAsync("T-REP");

            Assert.NotNull(item);
            Assert.IsType<UnderRepairState>(item.CurrentState);
        }

        // ── State transition via ItemLifecycle ──────────────────────────────

        [Fact]
        public async Task Rent_ViaItemLifecycle_TransitionsToRentedAndPersists()
        {
            var item = new ToolItem("T-TRN", "Test Tool", 10m); // starts AvailableState
            _writeCtx.RentalItems.Add(item);
            await _writeCtx.SaveChangesAsync();

            // Drive the State pattern Context: Available → Rented
            var lifecycle = new ItemLifecycle(
                item.CurrentState,
                NullLogger<ItemLifecycle>.Instance);
            lifecycle.Rent();
            item.CurrentState = lifecycle.CurrentState; // write back to tracked entity
            await _writeCtx.SaveChangesAsync();         // issues UPDATE via ValueConverter

            await using var fresh = DbContextFactory.CreateFresh(_connection);
            var reloaded = await fresh.RentalItems.FindAsync("T-TRN");

            Assert.NotNull(reloaded);
            Assert.IsType<RentedState>(reloaded.CurrentState);
        }

        [Fact]
        public void Rent_WhenAlreadyRented_ThrowsInvalidOperationException()
        {
            var lifecycle = new ItemLifecycle(
                new RentedState(),
                NullLogger<ItemLifecycle>.Instance);

            Assert.Throws<InvalidOperationException>(() => lifecycle.Rent());
        }

        [Fact]
        public void Return_WhenAvailable_ThrowsInvalidOperationException()
        {
            var lifecycle = new ItemLifecycle(
                new AvailableState(),
                NullLogger<ItemLifecycle>.Instance);

            Assert.Throws<InvalidOperationException>(() => lifecycle.Return());
        }

        [Fact]
        public void Return_WhenUnderRepair_ThrowsInvalidOperationException()
        {
            var lifecycle = new ItemLifecycle(
                new UnderRepairState(),
                NullLogger<ItemLifecycle>.Instance);

            Assert.Throws<InvalidOperationException>(() => lifecycle.Return());
        }

        public void Dispose()
        {
            _writeCtx.Dispose();
            _connection.Dispose();
        }
    }
}
