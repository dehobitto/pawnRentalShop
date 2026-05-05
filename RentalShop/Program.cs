using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using RentalShop.Application.Builders;
using RentalShop.Application.Commands;
using RentalShop.Application.Facades;
using RentalShop.Application.Factories;
using RentalShop.Application.Services;
using RentalShop.Domain.Entities;
using RentalShop.Domain.States;
using RentalShop.Infrastructure.Repositories;

namespace RentalShop
{
    /// <summary>
    /// Composition root for the Rental Shop Management System demo.
    /// Walks each of the 10 GoF patterns individually, then drives the
    /// cross-pattern <see cref="RentalShopFacade"/> end-to-end.
    /// </summary>
    internal static class Program
    {
        private static async Task Main()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
                builder.AddConsole().SetMinimumLevel(LogLevel.Debug));

            DemoFactoryMethod(out var tools, out var gear);
            DemoBuilder();
            DemoComposite();
            var inner = await DemoProxy(tools, gear, loggerFactory);
            DemoState(loggerFactory);
            DemoStrategy(loggerFactory);
            DemoObserver(loggerFactory);
            DemoTemplateMethod(loggerFactory);
            await DemoCommand(loggerFactory);
            await DemoFacade(inner, loggerFactory);

            Console.WriteLine("\n=== Demo complete ===");
        }

        private static void DemoFactoryMethod(out RentalItem[] tools, out RentalItem[] gear)
        {
            Header("1. Factory Method — instantiate rental entities");
            ItemFactory toolFactory = new ToolFactory();
            ItemFactory gearFactory = new GearFactory();

            tools = new[] { toolFactory.Create(), toolFactory.Create(), toolFactory.Create() };
            gear  = new[] { gearFactory.Create(), gearFactory.Create(), gearFactory.Create() };

            foreach (var p in tools) Console.WriteLine($"  Created: {p}");
            foreach (var p in gear)  Console.WriteLine($"  Created: {p}");
        }

        private static void DemoBuilder()
        {
            Header("2. Builder — assemble rental orders step-by-step");
            var director = new OrderDirector();

            OrderBuilder standard = new StandardOrderBuilder();
            director.Build(standard);
            Console.WriteLine("  Standard order:");
            standard.GetOrder().Show();

            OrderBuilder premium = new PremiumOrderBuilder();
            director.Build(premium);
            Console.WriteLine("  Premium order:");
            premium.GetOrder().Show();
        }

        private static void DemoComposite()
        {
            Header("3. Composite — uniform pricing for items vs. packages");
            var root = new RentalPackage("Camping Bundle");
            root.Add(new RentalLineItem("Camping Tent", 22m));
            root.Add(new RentalLineItem("Sleeping Bag", 8m));

            var subPackage = new RentalPackage("Cooking Kit");
            subPackage.Add(new RentalLineItem("Camp Stove", 5m));
            subPackage.Add(new RentalLineItem("Cookware Set", 4m));
            root.Add(subPackage);

            root.Display(1);
            Console.WriteLine($"  TOTAL: €{root.GetPrice()}");
        }

        private static async Task<InMemoryCatalogRepository> DemoProxy(
            RentalItem[] tools, RentalItem[] gear, ILoggerFactory loggerFactory)
        {
            Header("4. Proxy — caching wrapper around the mock catalog repo");
            var inner = new InMemoryCatalogRepository(
                loggerFactory.CreateLogger<InMemoryCatalogRepository>());
            foreach (var p in tools) inner.Seed(p);
            foreach (var p in gear)  inner.Seed(p);

            ICatalogRepository catalog = new CachingCatalogRepository(inner,
                loggerFactory.CreateLogger<CachingCatalogRepository>());

            await catalog.FindAsync("TOOL-001");
            await catalog.FindAsync("TOOL-001");
            await catalog.FindAsync("GEAR-002");
            await catalog.FindAsync("MISSING-999");

            return inner;
        }

        private static void DemoState(ILoggerFactory loggerFactory)
        {
            Header("5. State — item lifecycle (Available / Rented / Under Repair)");
            var lifecycle = new ItemLifecycle(new AvailableState(),
                loggerFactory.CreateLogger<ItemLifecycle>());
            lifecycle.Advance();
            lifecycle.Advance();
            Console.WriteLine("  -- damage reported --");
            lifecycle.CurrentState = new UnderRepairState();
            lifecycle.Advance();
        }

        private static void DemoStrategy(ILoggerFactory loggerFactory)
        {
            Header("6. Strategy — swappable rental-pricing rules");
            var calculator = new PricingCalculator(new StandardPricingStrategy(),
                loggerFactory.CreateLogger<PricingCalculator>());
            calculator.Calculate(20m, 3);
            calculator.SetStrategy(new WeekendPricingStrategy());
            calculator.Calculate(20m, 3);
            calculator.SetStrategy(new LoyaltyPricingStrategy());
            calculator.Calculate(20m, 3);
        }

        private static void DemoObserver(ILoggerFactory loggerFactory)
        {
            Header("7. Observer — waitlist notification on item return");
            var waitlist = new ItemWaitlist("TOOL-001");
            waitlist.Subscribe(new CustomerSubscriber(waitlist, "Alice",
                loggerFactory.CreateLogger<CustomerSubscriber>()));
            waitlist.Subscribe(new CustomerSubscriber(waitlist, "Bob",
                loggerFactory.CreateLogger<CustomerSubscriber>()));
            waitlist.Subscribe(new CustomerSubscriber(waitlist, "Carol",
                loggerFactory.CreateLogger<CustomerSubscriber>()));

            Console.WriteLine("  -- item returned, broadcasting waitlist --");
            waitlist.Status = "Available";
        }

        private static void DemoTemplateMethod(ILoggerFactory loggerFactory)
        {
            Header("8. Template Method — receipt vs. contract document");

            Console.WriteLine("  Receipt:");
            DocumentRenderer receipt = new ReceiptDocument(
                loggerFactory.CreateLogger<ReceiptDocument>());
            receipt.Render();

            Console.WriteLine("\n  Contract:");
            DocumentRenderer contract = new ContractDocument(
                loggerFactory.CreateLogger<ContractDocument>());
            contract.Render();
        }

        private static async Task DemoCommand(ILoggerFactory loggerFactory)
        {
            Header("9. Command — cashier actions, queueable & undoable");
            var terminal = new CashierTerminal(loggerFactory.CreateLogger<CashierTerminal>());
            var console  = new CashierConsole(loggerFactory.CreateLogger<CashierConsole>());

            console.Stage(new RentItemCommand(terminal, "TOOL-001",
                loggerFactory.CreateLogger<RentItemCommand>()));
            await console.ExecuteStagedAsync();

            console.Stage(new ReturnItemCommand(terminal, "GEAR-002",
                loggerFactory.CreateLogger<ReturnItemCommand>()));
            await console.ExecuteStagedAsync();

            Console.WriteLine("  -- customer voids the last two actions --");
            await console.UndoLastAsync();
            await console.UndoLastAsync();
            await console.UndoLastAsync();   // safe: nothing left → logs warning
        }

        private static async Task DemoFacade(
            InMemoryCatalogRepository seededRepo, ILoggerFactory loggerFactory)
        {
            Header("10. Facade — single API the UI calls (cross-pattern hub)");

            ICatalogRepository catalog = new CachingCatalogRepository(seededRepo,
                loggerFactory.CreateLogger<CachingCatalogRepository>());
            var pricing  = new PricingCalculator(new WeekendPricingStrategy(),
                loggerFactory.CreateLogger<PricingCalculator>());

            DocumentRenderer contract = new ContractDocument(
                loggerFactory.CreateLogger<ContractDocument>());
            DocumentRenderer receipt  = new ReceiptDocument(
                loggerFactory.CreateLogger<ReceiptDocument>());

            var inventory = new InventoryService(catalog,
                loggerFactory.CreateLogger<InventoryService>());
            var billing   = new BillingService(pricing,
                loggerFactory.CreateLogger<BillingService>());
            var payment   = new PaymentService(
                loggerFactory.CreateLogger<PaymentService>());
            var document  = new DocumentService(contract,
                loggerFactory.CreateLogger<DocumentService>());

            var facade = new RentalShopFacade(
                inventory, billing, payment, document, contract, receipt,
                loggerFactory.CreateLogger<RentalShopFacade>());

            await facade.ProcessRentalAsync("TOOL-001", 3);
            await facade.ProcessReturnAsync("TOOL-001");
        }

        private static void Header(string title)
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 70));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('=', 70));
        }
    }
}
