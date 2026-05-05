using System.Threading;
using RentalShop.Domain.Entities;

namespace RentalShop.Application.Factories
{
    /// <summary>
    /// Represents a 'ConcreteCreator' in the Factory Method GoF pattern (variant B).
    ///
    /// Domain role: factory for the "gear" category — produces
    /// <see cref="GearItem"/> instances with auto-numbered SKUs.
    /// </summary>
    public class GearFactory : ItemFactory
    {
        private static readonly (string Name, decimal Price)[] Templates =
        {
            ("Camping Tent", 22m),
            ("Sleeping Bag", 8m),
            ("Kayak", 35m),
        };

        private int _counter;

        public override RentalItem Create()
        {
            var n = Interlocked.Increment(ref _counter);
            var template = Templates[(n - 1) % Templates.Length];
            return new GearItem($"GEAR-{n:D3}", template.Name, template.Price);
        }
    }
}
