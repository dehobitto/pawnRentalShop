namespace RentalShop.Domain.States
{
    /// <summary>
    /// Represents the abstract 'State' in the State GoF pattern — declares
    /// the interface for encapsulating behaviour associated with each lifecycle
    /// state of an item.
    ///
    /// Domain role: lifecycle-state contract. Each concrete state handles the
    /// same domain events differently depending on context.
    /// </summary>
    public interface IItemState
    {
        /// <summary>
        /// Represents the 'Handle' operation in the State GoF pattern for a
        /// rent event. Transitions the context to <see cref="RentedState"/>
        /// when the current state allows it; throws otherwise.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the current state does not permit renting.
        /// </exception>
        void Rent(ItemLifecycle lifecycle);

        /// <summary>
        /// Represents the 'Handle' operation in the State GoF pattern for a
        /// return event. Transitions the context to <see cref="AvailableState"/>
        /// when the current state allows it; throws otherwise.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the current state does not permit returning.
        /// </exception>
        void Return(ItemLifecycle lifecycle);
    }
}
