using RentalShop.Domain.Entities;

namespace RentalShop.Application.Builders
{
    /// <summary>
    /// Represents a 'ConcreteBuilder' in the Builder GoF pattern (variant 1).
    ///
    /// Domain role: assembles a standard rental order — basic line items, no
    /// extras beyond the deposit.
    /// </summary>
    public class StandardOrderBuilder : OrderBuilder
    {
        private const string DepositLine = "Extra: Basic deposit (€50)";

        private readonly RentalOrder _order = new();

        public override void AddItems()
        {
            _order.AddPart("Line item: Cordless Drill × 1");
            _order.AddPart("Line item: Hammer-set × 1");
        }

        public override void AddExtras() => _order.AddPart(DepositLine);

        public override RentalOrder GetOrder() => _order;
    }
}
