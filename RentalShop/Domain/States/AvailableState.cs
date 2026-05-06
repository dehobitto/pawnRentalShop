using System;

namespace RentalShop.Domain.States
{
    /// <summary>
    /// Represents a 'ConcreteState' in the State GoF pattern.
    ///
    /// Domain role: "Available" — the item is on the shelf and may be rented.
    /// Renting transitions the context to <see cref="RentedState"/>;
    /// attempting to return an already-available item is a domain error.
    /// </summary>
    public class AvailableState : IItemState
    {
        /// <inheritdoc/>
        public void Rent(ItemLifecycle lifecycle)
            => lifecycle.CurrentState = new RentedState();

        /// <inheritdoc/>
        public void Return(ItemLifecycle lifecycle)
            => throw new InvalidOperationException("Item is not currently rented and cannot be returned.");
    }
}
