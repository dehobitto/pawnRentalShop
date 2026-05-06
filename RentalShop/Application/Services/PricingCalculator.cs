using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents the 'Context' in the Strategy GoF pattern — holds a
    /// strategy reference and exposes a stable call-site that delegates to
    /// it.
    ///
    /// Domain role: pricing engine. Holds the active
    /// <see cref="IPricingStrategy"/> and gives the rest of the domain a
    /// single, stable entry point for price calculation. Logging lives here
    /// so the strategy implementations remain pure calculation objects
    /// with no infrastructure concerns.
    /// </summary>
    public class PricingCalculator
    {
        private IPricingStrategy _strategy;
        private readonly ILogger<PricingCalculator> _logger;

        public PricingCalculator(IPricingStrategy strategy, ILogger<PricingCalculator> logger)
        {
            _strategy = strategy;
            _logger = logger;
        }

        /// <summary>Swap the active pricing strategy at runtime.</summary>
        public void SetStrategy(IPricingStrategy strategy) => _strategy = strategy;

        /// <summary>Triggers price calculation under the currently configured strategy.</summary>
        public decimal Calculate(decimal baseRate, int days)
        {
            var total = _strategy.Calculate(baseRate, days);
            _logger.LogDebug("[Pattern: Strategy] {Strategy}: €{BaseRate}/day × {Days}d = €{Total}",
                _strategy.GetType().Name, baseRate, days, total);
            return total;
        }
    }
}
