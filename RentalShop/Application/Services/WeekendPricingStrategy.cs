using Microsoft.Extensions.Options;
using RentalShop.Models;

namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents a 'ConcreteStrategy' in the Strategy GoF pattern (variant B).
    ///
    /// Domain role: weekend pricing — base rate × days × a configurable surcharge
    /// multiplier. The multiplier is loaded from
    /// <see cref="RentalShopSettings.WeekendMultiplier"/> via
    /// <c>IOptions&lt;RentalShopSettings&gt;</c> so it can be changed in
    /// <c>appsettings.json</c> without recompilation.
    ///
    /// Note: the multiplier is applied uniformly to every day in the rental
    /// period regardless of actual day-of-week; no <see cref="System.DayOfWeek"/>
    /// check is performed. The strategy is selected by the caller.
    /// Pure calculation; logging is handled by the <see cref="PricingCalculator"/> context.
    /// </summary>
    public class WeekendPricingStrategy : IPricingStrategy
    {
        private readonly decimal _multiplier;

        public WeekendPricingStrategy(IOptions<RentalShopSettings> options)
            => _multiplier = options.Value.WeekendMultiplier;

        public decimal Calculate(decimal baseRate, int days) => baseRate * days * _multiplier;
    }
}
