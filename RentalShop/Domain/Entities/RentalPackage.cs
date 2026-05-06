using System.Collections.Generic;

namespace RentalShop.Domain.Entities
{
    /// <summary>
    /// Represents the 'Composite' in the Composite GoF pattern — a node that
    /// holds children and aggregates their behaviour.
    ///
    /// Domain role: a rental package that bundles several items (or other
    /// packages); its total price is the recursive sum of its children.
    ///
    /// Persistence note: EF Core accesses the <c>_children</c> backing field
    /// directly via a named navigation (<c>HasMany("_children")</c>) with a
    /// shadow FK <c>ParentPackageId</c> on <see cref="PackageComponent"/>.
    /// </summary>
    public class RentalPackage : PackageComponent
    {
        private List<PackageComponent> _children = new();

        public RentalPackage(string name) : base(name) { }

        /// <summary>Parameterless constructor required by EF Core for entity materialisation.</summary>
        private RentalPackage() { }

        public override void Add(PackageComponent child) => _children.Add(child);
        public override void Remove(PackageComponent child) => _children.Remove(child);

        /// <inheritdoc/>
        public override IEnumerable<string> GetDisplayLines(int depth = 0)
        {
            yield return $"{Indent(depth)}[{Name}] €{GetPrice()}";
            foreach (var child in _children)
                foreach (var line in child.GetDisplayLines(depth + 1))
                    yield return line;
        }

        /// <inheritdoc/>
        public override decimal GetPrice()
        {
            decimal total = 0m;
            foreach (var child in _children) total += child.GetPrice();
            return total;
        }
    }
}
