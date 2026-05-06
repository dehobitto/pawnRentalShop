using Microsoft.Extensions.Options;
using RentalShop.Models;

namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents a 'ConcreteStrategy' in the Strategy GoF pattern (variant C).
    ///
    /// Domain role: loyalty / repeat-customer pricing — applies a configurable
    /// discount on top of the standard base rate. The multiplier is loaded from
    /// <see cref="RentalShopSettings.LoyaltyMultiplier"/> via
    /// <c>IOptions&lt;RentalShopSettings&gt;</c> so it can be changed in
    /// <c>appsettings.json</c> without recompilation.
    /// Pure calculation; logging is handled by the <see cref="PricingCalculator"/> context.
    /// </summary>
    public class LoyaltyPricingStrategy : IPricingStrategy
    {
        private readonly decimal _multiplier;

        public LoyaltyPricingStrategy(IOptions<RentalShopSettings> options)
            => _multiplier = options.Value.LoyaltyMultiplier;

        public decimal Calculate(decimal baseRate, int days) => baseRate * days * _multiplier;
    }
}
