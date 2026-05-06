using System;

namespace RentalShop.Domain.States
{
    /// <summary>
    /// Represents a 'ConcreteState' in the State GoF pattern (extension over
    /// the canonical A/B example).
    ///
    /// Domain role: "Under Repair" — the item is in the workshop and is
    /// unavailable to customers. Neither renting nor returning is permitted
    /// until a repair-complete transition restores it to
    /// <see cref="AvailableState"/> via a separate workflow.
    /// </summary>
    public class UnderRepairState : IItemState
    {
        /// <inheritdoc/>
        public void Rent(ItemLifecycle lifecycle)
            => throw new InvalidOperationException("Item is under repair and cannot be rented.");

        /// <inheritdoc/>
        public void Return(ItemLifecycle lifecycle)
            => throw new InvalidOperationException("Item is under repair and cannot be returned.");
    }
}
