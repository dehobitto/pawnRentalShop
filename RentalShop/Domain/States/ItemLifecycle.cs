using Microsoft.Extensions.Logging;

namespace RentalShop.Domain.States
{
    /// <summary>
    /// Represents the 'Context' in the State GoF pattern — the object whose
    /// behaviour changes as its current <see cref="IItemState"/> changes.
    ///
    /// Domain role: owns the current lifecycle state of a single rentable
    /// item. Callers invoke <see cref="Rent"/> or <see cref="Return"/>; what
    /// actually happens (or whether an exception is raised) is decided by the
    /// active state object.
    /// </summary>
    public class ItemLifecycle
    {
        private IItemState _state;
        private readonly ILogger<ItemLifecycle> _logger;

        public ItemLifecycle(IItemState initialState, ILogger<ItemLifecycle> logger)
        {
            _state = initialState;
            _logger = logger;
            _logger.LogDebug("[Pattern: State] Initial state: {State}", StateName(_state));
        }

        /// <summary>
        /// Active lifecycle state. The setter logs every transition so the
        /// full audit trail is available in structured logs.
        /// </summary>
        public IItemState CurrentState
        {
            get => _state;
            set
            {
                _logger.LogDebug("[Pattern: State] Transition: {From} → {To}",
                    StateName(_state), StateName(value));
                _state = value;
            }
        }

        /// <summary>
        /// Represents the 'Request' entry-point in the State GoF pattern for
        /// a rent event. Delegates to the active state's <see cref="IItemState.Rent"/> method.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Propagated from the active state when renting is not permitted.
        /// </exception>
        public void Rent() => _state.Rent(this);

        /// <summary>
        /// Represents the 'Request' entry-point in the State GoF pattern for
        /// a return event. Delegates to the active state's <see cref="IItemState.Return"/> method.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Propagated from the active state when returning is not permitted.
        /// </exception>
        public void Return() => _state.Return(this);

        private static string StateName(IItemState state) => state.GetType().Name;
    }
}
