using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

        /// <summary>
        /// Logger used to record subscriber errors during notification.
        /// Defaults to <see cref="NullLogger.Instance"/> so subclasses that
        /// do not inject a logger remain silent rather than crashing.
        /// </summary>
        protected ILogger Logger { get; set; } = NullLogger.Instance;

        /// <summary>Adds a customer to this item's waitlist (duplicate-safe).</summary>
        public void Subscribe(IWaitlistSubscriber subscriber)
        {
            lock (_syncRoot)
            {
                if (!_subscribers.Contains(subscriber))
                    _subscribers.Add(subscriber);
            }
        }

        /// <summary>Removes a customer from the waitlist (cancellation or fulfilment).</summary>
        public void Unsubscribe(IWaitlistSubscriber subscriber)
        {
            lock (_syncRoot) _subscribers.Remove(subscriber);
        }

        /// <summary>
        /// Pushes the current item state to every subscriber on the waitlist.
        /// A failing subscriber is logged and skipped so the remaining
        /// observers still receive their notification.
        /// </summary>
        public void NotifyAll()
        {
            List<IWaitlistSubscriber> snapshot;
            lock (_syncRoot) snapshot = new List<IWaitlistSubscriber>(_subscribers);
            foreach (var subscriber in snapshot)
            {
                try
                {
                    subscriber.OnItemAvailable();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex,
                        "[Pattern: Observer] Subscriber {Type} threw during notification — skipped",
                        subscriber.GetType().Name);
                }
            }
        }
    }
}
