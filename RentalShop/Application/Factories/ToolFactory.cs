using System.Threading;
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
    public class ToolFactory : ItemFactory
    {
        private static readonly (string Name, decimal Price)[] Templates =
        {
            ("Cordless Drill", 12m),
            ("Circular Saw", 18m),
            ("Hammer-set", 6m),
        };

        private int _counter;

        public override RentalItem Create()
        {
            var n = Interlocked.Increment(ref _counter);
            var template = Templates[(n - 1) % Templates.Length];
            return new ToolItem($"TOOL-{n:D3}", template.Name, template.Price);
        }
    }
}
