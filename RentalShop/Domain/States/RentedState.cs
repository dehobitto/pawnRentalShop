using System;

namespace RentalShop.Domain.States
{
    /// <summary>
    /// Represents a 'ConcreteState' in the State GoF pattern.
    ///
    /// Domain role: "Rented" — the item is currently with a customer.
    /// Returning transitions the context back to <see cref="AvailableState"/>;
    /// attempting to rent an already-rented item is a domain error.
    /// </summary>
    public class RentedState : IItemState
    {
        /// <inheritdoc/>
        public void Rent(ItemLifecycle lifecycle)
            => throw new InvalidOperationException("Item is already rented.");

        /// <inheritdoc/>
        public void Return(ItemLifecycle lifecycle)
            => lifecycle.CurrentState = new AvailableState();
    }
}
