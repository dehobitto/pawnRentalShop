using RentalShop.Domain.Entities;

namespace RentalShop.Application.Factories
{
    /// <summary>
    /// Represents a 'ConcreteCreator' in the Factory Method GoF pattern (variant A).
    ///
    /// Domain role: factory for the "tools" category — produces
    /// <see cref="ToolItem"/> instances with auto-numbered SKUs drawn from a
    /// small in-memory template pool.
    /// </summary>
    public class ToolFactory : TemplatedItemFactory
    {
        protected override (string Name, decimal Price)[] Templates { get; } =
        {
            ("Cordless Drill", 12m),
            ("Circular Saw",   18m),
            ("Hammer-set",      6m),
        };

        protected override string SkuPrefix => "TOOL";

        protected override RentalItem BuildItem(string sku, string name, decimal price)
            => new ToolItem(sku, name, price);
    }
}
