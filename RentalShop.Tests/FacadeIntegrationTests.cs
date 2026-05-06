using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using RentalShop.Application.Facades;
using RentalShop.Application.Factories;
using RentalShop.Application.Services;
using RentalShop.Domain.States;
using RentalShop.Infrastructure.Repositories;
using RentalShop.Tests.Helpers;

namespace RentalShop.Tests
{
    /// <summary>
    /// End-to-end integration tests for the <b>Facade GoF pattern</b>.
    ///
    /// Wires the full dependency graph — <see cref="EfCoreCatalogRepository"/>
    /// (Proxy RealSubject) → <see cref="InventoryService"/> → <see cref="BillingService"/>
    /// → <see cref="PaymentService"/> → <see cref="DocumentService"/> →
    /// <see cref="RentalShopFacade"/> — against an SQLite in-memory database
    /// seeded with the 6 catalog items defined in <c>RentalDbContext.SeedCatalog</c>.
    ///
    /// All loggers are <c>NullLogger</c> so tests are silent; only domain
    /// correctness is asserted.
    /// </summary>
    public sealed class FacadeIntegrationTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly RentalShop.Infrastructure.Persistence.RentalDbContext _ctx;
        private readonly RentalShopFacade _facade;

        public FacadeIntegrationTests()
        {
            (_connection, _ctx) = DbContextFactory.Create();
            _facade = BuildFacade(_ctx);
        }

        // ── Rent flow ────────────────────────────────────────────────────────

        [Fact]
        public async Task ProcessRentalAsync_KnownSku_CompletesWithoutException()
        {
            var exception = await Record.ExceptionAsync(
                () => _facade.ProcessRentalAsync("TOOL-001", 3));

            Assert.Null(exception);
        }

        [Fact]
        public async Task ProcessRentalAsync_KnownGearSku_CompletesWithoutException()
        {
            var exception = await Record.ExceptionAsync(
                () => _facade.ProcessRentalAsync("GEAR-003", 5));

            Assert.Null(exception);
        }

        [Fact]
        public async Task ProcessRentalAsync_AlreadyRentedSku_ReturnsFalse()
        {
            await _facade.ProcessRentalAsync("TOOL-003", 1); // first rent succeeds
            var result = await _facade.ProcessRentalAsync("TOOL-003", 1); // second must fail

            Assert.False(result);
        }

        // ── Return flow ──────────────────────────────────────────────────────

        [Fact]
        public async Task ProcessReturnAsync_KnownSku_CompletesWithoutException()
        {
            await _facade.ProcessRentalAsync("TOOL-002", 1); // must rent first
            var exception = await Record.ExceptionAsync(
                () => _facade.ProcessReturnAsync("TOOL-002"));

            Assert.Null(exception);
        }

        [Fact]
        public async Task ProcessReturnAsync_NotRentedSku_ReturnsFalse()
        {
            // GEAR-002 is in AvailableState — returning it must be rejected
            var result = await _facade.ProcessReturnAsync("GEAR-002");

            Assert.False(result);
        }

        // ── Full cycle ───────────────────────────────────────────────────────

        [Fact]
        public async Task FullCycle_RentThenReturn_BothStepsCompleteWithoutException()
        {
            await _facade.ProcessRentalAsync("GEAR-001", 2);
            var exception = await Record.ExceptionAsync(
                () => _facade.ProcessReturnAsync("GEAR-001"));

            Assert.Null(exception);
        }

        // ── Unknown SKU ──────────────────────────────────────────────────────

        [Fact]
        public async Task ProcessRentalAsync_UnknownSku_ReturnsFalse()
        {
            var result = await _facade.ProcessRentalAsync("UNKNOWN-SKU", 3);

            Assert.False(result);
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private static RentalShopFacade BuildFacade(
            RentalShop.Infrastructure.Persistence.RentalDbContext ctx)
        {
            var repo      = new EfCoreCatalogRepository(ctx, NullLogger<EfCoreCatalogRepository>.Instance);
            var inventory = new InventoryService(repo,
                NullLogger<InventoryService>.Instance,
                NullLogger<ItemLifecycle>.Instance);
            var pricing   = new PricingCalculator(new StandardPricingStrategy(), NullLogger<PricingCalculator>.Instance);
            var billing   = new BillingService(pricing, NullLogger<BillingService>.Instance);
            var payment   = new PaymentService(NullLogger<PaymentService>.Instance);
            var contract  = new ContractDocument(NullLogger<ContractDocument>.Instance);
            var receipt   = new ReceiptDocument(NullLogger<ReceiptDocument>.Instance);
            var docSvc    = new DocumentService(receipt, NullLogger<DocumentService>.Instance);

            return new RentalShopFacade(
                inventory, billing, payment, docSvc,
                new DocumentRenderers(contract, receipt),
                new ToolFactory(), new GearFactory(),
                NullLogger<RentalShopFacade>.Instance);
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _connection.Dispose();
        }
    }
}
