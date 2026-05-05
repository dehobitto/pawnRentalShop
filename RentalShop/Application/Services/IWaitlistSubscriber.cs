namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents the 'Observer' in the Observer GoF pattern — the contract
    /// every subscriber implements to receive updates from the subject.
    ///
    /// Domain role: contract for "interested customers" subscribed to a
    /// waitlisted item.
    /// </summary>
    public interface IWaitlistSubscriber
    {
        /// <summary>
        /// Called when the watched item changes — implementations turn this
        /// into a real-world side-effect (email, SMS, …).
        /// </summary>
        void OnItemAvailable();
    }
}
