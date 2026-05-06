using Microsoft.Extensions.Logging;
using RentalShop.Application.Services;

namespace RentalShop.Application.Facades
{
    /// <summary>
    /// Represents a 'SubSystem' in the Facade GoF pattern (subsystem #2).
    ///
    /// Domain role: billing subsystem — applies the active pricing
    /// <see cref="PricingCalculator"/> and returns the line total.
    /// </summary>
    public class BillingService
    {
        private readonly PricingCalculator _pricing;
        private readonly ILogger<BillingService> _logger;

        public BillingService(PricingCalculator pricing, ILogger<BillingService> logger)
        {
            _pricing = pricing;
            _logger = logger;
        }

        /// <summary>
        /// Compute the order total using the currently configured pricing strategy.
        /// </summary>
        public decimal CalculateTotal(decimal baseRate, int days)
        {
            _logger.LogDebug("[Pattern: Facade] BillingService.CalculateTotal: €{BaseRate}/day × {Days}d",
                baseRate, days);
            return _pricing.Calculate(baseRate, days);
        }
    }
}
