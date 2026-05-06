using RentalShop.Domain.Entities;

namespace RentalShop.Application.Factories
{
    /// <summary>
    /// Represents a 'ConcreteCreator' in the Factory Method GoF pattern (variant B).
    ///
    /// Domain role: factory for the "gear" category — produces
    /// <see cref="GearItem"/> instances with auto-numbered SKUs.
    /// </summary>
    public class GearFactory : TemplatedItemFactory
    {
        protected override (string Name, decimal Price)[] Templates { get; } =
        {
            ("Camping Tent", 22m),
            ("Sleeping Bag",  8m),
            ("Kayak",        35m),
        };

        protected override string SkuPrefix => "GEAR";

        protected override RentalItem BuildItem(string sku, string name, decimal price)
            => new GearItem(sku, name, price);
    }
}
