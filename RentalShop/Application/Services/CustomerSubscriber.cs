using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents the 'ConcreteObserver' in the Observer GoF pattern.
    ///
    /// Domain role: a specific customer waiting for a specific item. On
    /// notification it reads the current <see cref="ItemWaitlist.Status"/>
    /// and dispatches the update via structured logging.
    /// </summary>
    public class CustomerSubscriber : IWaitlistSubscriber
    {
        private readonly string _name;
        private readonly ItemWaitlist _waitlist;
        private readonly ILogger<CustomerSubscriber> _logger;

        public CustomerSubscriber(ItemWaitlist waitlist, string name, ILogger<CustomerSubscriber> logger)
        {
            _waitlist = waitlist;
            _name     = name;
            _logger   = logger;
        }

        public void OnItemAvailable()
        {
            _logger.LogDebug("[Pattern: Observer] ConcreteObserver notified: {Name} for {Sku}",
                _name, _waitlist.Sku);
            _logger.LogInformation(
                "Waitlist notification logged for {CustomerName} — item {Sku} is available",
                _name, _waitlist.Sku);
        }
    }
}
