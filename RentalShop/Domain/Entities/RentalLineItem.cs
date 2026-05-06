using System.Collections.Generic;

namespace RentalShop.Domain.Entities
{
    /// <summary>
    /// Represents the 'Leaf' in the Composite GoF pattern — a node with no
    /// children that defines the base behaviour.
    ///
    /// Domain role: a single rental line item (one name, one daily rate).
    /// </summary>
    public class RentalLineItem : PackageComponent
    {
        /// <summary>
        /// Daily rental price for this line item.
        /// <c>private set</c> allows EF Core to materialise the value while
        /// keeping direct assignment outside the class impossible.
        /// </summary>
        public decimal Price { get; private set; }

        public RentalLineItem(string name, decimal price) : base(name)
        {
            Price = price;
        }

        /// <summary>Parameterless constructor required by EF Core for entity materialisation.</summary>
        private RentalLineItem() { }

        public override void Add(PackageComponent child) { }
        public override void Remove(PackageComponent child) { }

        /// <inheritdoc/>
        public override IEnumerable<string> GetDisplayLines(int depth = 0)
        {
            yield return $"{Indent(depth)}• {Name} — €{Price}/day";
        }

        /// <inheritdoc/>
        public override decimal GetPrice() => Price;
    }
}
