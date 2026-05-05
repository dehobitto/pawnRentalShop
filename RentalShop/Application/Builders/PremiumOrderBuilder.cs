using RentalShop.Domain.Entities;

namespace RentalShop.Application.Builders
{
    /// <summary>
    /// Represents a 'ConcreteBuilder' in the Builder GoF pattern (variant 2).
    ///
    /// Domain role: assembles a premium rental order — extra gear, insurance,
    /// delivery, and a loyalty discount.
    /// </summary>
    public class PremiumOrderBuilder : OrderBuilder
    {
        private readonly RentalOrder _order = new();

        public override void AddItems()
        {
            _order.AddPart("Line item: Camping Tent × 1");
            _order.AddPart("Line item: Sleeping Bag × 2");
            _order.AddPart("Line item: Kayak × 1");
        }

        public override void AddExtras()
        {
            _order.AddPart("Extra: Insurance (€15)");
            _order.AddPart("Extra: Delivery (€10)");
            _order.AddPart("Extra: Loyalty discount (−10%)");
        }

        public override RentalOrder GetOrder() => _order;
    }
}
