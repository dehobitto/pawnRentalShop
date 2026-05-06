using System;
using System.Threading;
using RentalShop.Domain.Entities;

namespace RentalShop.Application.Factories
{
    /// <summary>
    /// Intermediate abstract 'Creator' in the Factory Method GoF pattern —
    /// eliminates the duplicated auto-numbering loop shared by every
    /// template-driven factory (<see cref="ToolFactory"/>, <see cref="GearFactory"/>).
    ///
    /// Subclasses provide only three things: the template pool, the SKU prefix,
    /// and the concrete <see cref="RentalItem"/> type to instantiate.
    /// </summary>
    public abstract class TemplatedItemFactory : ItemFactory
    {
        private int _counter;

        /// <summary>Pool of (name, price) pairs cycled by the auto-SKU variant.</summary>
        protected abstract (string Name, decimal Price)[] Templates { get; }

        /// <summary>Prefix prepended to every auto-generated SKU (e.g. "TOOL", "GEAR").</summary>
        protected abstract string SkuPrefix { get; }

        /// <summary>
        /// Instantiates the concrete <see cref="RentalItem"/> subtype.
        /// Subclasses control which domain type is produced.
        /// </summary>
        protected abstract RentalItem BuildItem(string sku, string name, decimal price);

        /// <inheritdoc/>
        public override RentalItem Create()
        {
            var n = Interlocked.Increment(ref _counter);
            var (name, price) = Templates[(n - 1) % Templates.Length];
            return BuildItem($"{SkuPrefix}-{n:D3}", name, price);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The SKU is generated from a GUID segment so it is collision-free
        /// with both seeded data and items created by other factory instances.
        /// Format: <c>{PREFIX}-{8-char uppercase hex}</c>, e.g. <c>TOOL-3A4F2B1C</c>.
        /// </remarks>
        public override RentalItem Create(string name, decimal price)
        {
            var id = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            return BuildItem($"{SkuPrefix}-{id}", name, price);
        }
    }
}
