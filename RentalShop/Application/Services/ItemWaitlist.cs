namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents the 'ConcreteSubject' in the Observer GoF pattern — stores
    /// the state observers care about and triggers notifications on change.
    ///
    /// Domain role: a specific waitlist-enabled rental item (e.g. a popular
    /// drill model). Setting <see cref="Status"/> auto-broadcasts to every
    /// waiting customer.
    /// </summary>
    public class ItemWaitlist : WaitlistPublisher
    {
        private string _status = string.Empty;

        public ItemWaitlist(string sku)
        {
            Sku = sku;
        }

        /// <summary>SKU of the waitlisted item; included in the notification payload.</summary>
        public string Sku { get; init; }

        /// <summary>
        /// Item's externally-visible status string. Setting it auto-triggers
        /// <see cref="WaitlistPublisher.NotifyAll"/>.
        /// </summary>
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyAll();
            }
        }
    }
}
