namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents a 'ConcreteStrategy' in the Strategy GoF pattern (variant B).
    ///
    /// Domain role: weekend pricing — base rate × days × 1.5 weekend
    /// multiplier. Pure calculation; logging is handled by the
    /// <see cref="PricingCalculator"/> context.
    /// </summary>
    public class WeekendPricingStrategy : IPricingStrategy
    {
        private const decimal WeekendMultiplier = 1.5m;

        public decimal Calculate(decimal baseRate, int days) => baseRate * days * WeekendMultiplier;
    }
}
