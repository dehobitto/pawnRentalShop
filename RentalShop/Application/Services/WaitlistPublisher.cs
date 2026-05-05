using System.Collections.Generic;

namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents the abstract 'Subject' in the Observer GoF pattern — owns
    /// the list of observers and exposes attach/detach/notify operations.
    ///
    /// Domain role: base class for items that maintain a customer waitlist.
    /// When the item state changes (e.g. on return), all subscribers are
    /// notified.
    /// </summary>
    public abstract class WaitlistPublisher
    {
        private readonly List<IWaitlistSubscriber> _subscribers = new();
        private readonly object _syncRoot = new();

        /// <summary>Adds a customer to this item's waitlist.</summary>
        public void Subscribe(IWaitlistSubscriber subscriber)
        {
            lock (_syncRoot) _subscribers.Add(subscriber);
        }

        /// <summary>Removes a customer from the waitlist (cancellation or fulfilment).</summary>
        public void Unsubscribe(IWaitlistSubscriber subscriber)
        {
            lock (_syncRoot) _subscribers.Remove(subscriber);
        }

        /// <summary>Pushes the current item state to every subscriber on the waitlist.</summary>
        public void NotifyAll()
        {
            List<IWaitlistSubscriber> snapshot;
            lock (_syncRoot) snapshot = new List<IWaitlistSubscriber>(_subscribers);
            foreach (var s in snapshot) s.OnItemAvailable();
        }
    }
}
