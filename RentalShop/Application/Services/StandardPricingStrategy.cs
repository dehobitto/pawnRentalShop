namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents a 'ConcreteStrategy' in the Strategy GoF pattern (variant A).
    ///
    /// Domain role: standard weekday pricing — base rate × days, no
    /// modifiers. Pure calculation; logging is handled by the
    /// <see cref="PricingCalculator"/> context.
    /// </summary>
    public class StandardPricingStrategy : IPricingStrategy
    {
        public decimal Calculate(decimal baseRate, int days) => baseRate * days;
    }
}
