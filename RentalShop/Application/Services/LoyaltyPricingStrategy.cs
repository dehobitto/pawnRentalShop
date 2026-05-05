namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents a 'ConcreteStrategy' in the Strategy GoF pattern (variant C).
    ///
    /// Domain role: loyalty / repeat-customer pricing — applies a 15%
    /// discount on top of the standard base rate. Pure calculation; logging
    /// is handled by the <see cref="PricingCalculator"/> context.
    /// </summary>
    public class LoyaltyPricingStrategy : IPricingStrategy
    {
        private const decimal LoyaltyDiscount = 0.85m;

        public decimal Calculate(decimal baseRate, int days) => baseRate * days * LoyaltyDiscount;
    }
}
