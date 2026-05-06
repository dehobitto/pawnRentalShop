using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using RentalShop.Domain.Entities;
using RentalShop.Tests.Helpers;

namespace RentalShop.Tests
{
    /// <summary>
    /// Tests for the <b>Composite GoF pattern</b> — both the pure domain
    /// behaviour (recursive price aggregation) and the DB round-trip that
    /// verifies the TPH mapping with the private <c>_children</c> backing
    /// field and the shadow FK <c>ParentPackageId</c>.
    /// </summary>
    public sealed class CompositePatternTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly RentalShop.Infrastructure.Persistence.RentalDbContext _writeCtx;

        public CompositePatternTests()
        {
            (_connection, _writeCtx) = DbContextFactory.Create();
        }

        // ── Pure domain behaviour ────────────────────────────────────────────

        [Fact]
        public void SingleLeaf_GetPrice_ReturnsItsOwnPrice()
        {
            var leaf = new RentalLineItem("Cordless Drill", 12m);

            Assert.Equal(12m, leaf.GetPrice());
        }

        [Fact]
        public void Package_WithTwoLeaves_GetPrice_ReturnsSumOfChildren()
        {
            var pkg = new RentalPackage("Tool Bundle");
            pkg.Add(new RentalLineItem("Cordless Drill", 12m));
            pkg.Add(new RentalLineItem("Circular Saw",   18m));

            Assert.Equal(30m, pkg.GetPrice());
        }

        [Fact]
        public void Package_GetDisplayLines_ListsAllNodes()
        {
            var pkg = new RentalPackage("Bundle");
            pkg.Add(new RentalLineItem("Drill", 12m));
            pkg.Add(new RentalLineItem("Saw",   18m));

            var lines = pkg.GetDisplayLines().ToList();

            Assert.Equal(3, lines.Count);                        // 1 root + 2 leaves
            Assert.Contains("Bundle",  lines[0]);
            Assert.Contains("Drill",   lines[1]);
            Assert.Contains("Saw",     lines[2]);
        }

        // ── DB round-trip ────────────────────────────────────────────────────

        [Fact]
        public async Task Package_WithLeaves_PersistedAndReloaded_GetPriceIsCorrect()
        {
            var pkg = new RentalPackage("Tool Bundle");
            pkg.Add(new RentalLineItem("Cordless Drill", 12m));
            pkg.Add(new RentalLineItem("Circular Saw",   18m));

            _writeCtx.PackageComponents.Add(pkg);
            await _writeCtx.SaveChangesAsync();
            var savedId = pkg.Id;

            await using var fresh = DbContextFactory.CreateFresh(_connection);
            var loaded = await fresh.Set<RentalPackage>()
                .Include("_children")
                .FirstOrDefaultAsync(p => p.Id == savedId);

            Assert.NotNull(loaded);
            Assert.Equal(30m, loaded.GetPrice());
        }

        [Fact]
        public async Task Package_WithLeaves_PersistedAndReloaded_DisplayLinesAreComplete()
        {
            var pkg = new RentalPackage("Camping Set");
            pkg.Add(new RentalLineItem("Camping Tent",  22m));
            pkg.Add(new RentalLineItem("Sleeping Bag",   8m));

            _writeCtx.PackageComponents.Add(pkg);
            await _writeCtx.SaveChangesAsync();
            var savedId = pkg.Id;

            await using var fresh = DbContextFactory.CreateFresh(_connection);
            var loaded = await fresh.Set<RentalPackage>()
                .Include("_children")
                .FirstOrDefaultAsync(p => p.Id == savedId);

            Assert.NotNull(loaded);
            var lines = loaded.GetDisplayLines().ToList();
            Assert.Equal(3, lines.Count);
            Assert.Contains("Camping Set",   lines[0]);
            Assert.Contains("Camping Tent",  lines[1]);
            Assert.Contains("Sleeping Bag",  lines[2]);
        }

        public void Dispose()
        {
            _writeCtx.Dispose();
            _connection.Dispose();
        }
    }
}
