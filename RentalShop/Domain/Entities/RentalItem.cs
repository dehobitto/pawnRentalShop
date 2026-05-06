using System;
using RentalShop.Domain.States;

namespace RentalShop.Domain.Entities
{
    /// <summary>
    /// Represents the abstract 'Product' in the Factory Method GoF pattern —
    /// the base type that concrete factories produce.
    ///
    /// Domain role: base abstraction for any rentable entity in the catalog
    /// (tool, gear, vehicle, …). Lets the rest of the domain treat catalog
    /// items uniformly.
    ///
    /// Persistence note: <see cref="CurrentState"/> is mapped to the string
    /// column <c>CurrentStateName</c> via an EF Core <c>ValueConverter</c> —
    /// demonstrating the State GoF pattern in the relational database layer.
    /// <see cref="Version"/> is the optimistic concurrency token; EF Core
    /// includes it in every UPDATE WHERE clause so a stale write is detected
    /// before it silently overwrites a concurrent change.
    /// </summary>
    public abstract class RentalItem
    {
        /// <summary>Stable catalog identifier; used for repository lookup and as the EF Core primary key.</summary>
        public string Sku { get; protected set; } = string.Empty;

        /// <summary>Human-readable name shown on receipts and contracts.</summary>
        public string Name { get; protected set; } = string.Empty;

        /// <summary>Base daily rate before any pricing strategy is applied.</summary>
        public decimal BasePricePerDay { get; protected set; }

        /// <summary>
        /// Current lifecycle state of this item.
        /// Persisted as the state class name via an EF Core <c>ValueConverter&lt;IItemState, string&gt;</c>;
        /// rehydrated into the matching concrete state object on entity materialisation.
        /// Demonstrates the State GoF pattern bridging to the ORM layer.
        /// </summary>
        public IItemState CurrentState { get; set; } = new AvailableState();

        /// <summary>
        /// Optimistic concurrency token. Must be bumped to a new <see cref="Guid"/>
        /// on every state-changing operation so EF Core can detect when two
        /// concurrent requests attempt to modify the same row simultaneously.
        ///
        /// EF Core generates:
        /// <c>UPDATE RentalItems SET … , Version = @new WHERE Sku = @sku AND Version = @original</c>
        /// If the DB row was already updated by another transaction (Version mismatch),
        /// 0 rows are affected and EF Core throws <c>DbUpdateConcurrencyException</c>.
        /// </summary>
        public Guid Version { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Set to <c>UtcNow + rental duration</c> when the item transitions to
        /// <see cref="RentalShop.Domain.States.RentedState"/>; cleared to <c>null</c>
        /// when the item is returned. Null when the item is Available or UnderRepair.
        /// </summary>
        public DateTime? ExpectedReturnDate { get; set; }

        public override string ToString() => $"{Sku} — {Name} (€{BasePricePerDay}/day)";
    }
}
