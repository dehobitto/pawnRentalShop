using System.Collections.Generic;

namespace RentalShop.Domain.Entities
{
    /// <summary>
    /// Represents the 'Product' in the Builder GoF pattern — the
    /// step-by-step-assembled object an OrderBuilder produces.
    ///
    /// Domain role: a fully-assembled rental order (line items + extras).
    /// Intentionally has no console/logging output — the caller logs the
    /// assembled order via the exposed <see cref="Parts"/> collection.
    /// </summary>
    public class RentalOrder
    {
        private readonly List<string> _parts = new();

        /// <summary>Appends a line item or extra during builder assembly.</summary>
        public void AddPart(string part) => _parts.Add(part);

        /// <summary>Read-only view of all assembled parts; used by callers for logging or rendering.</summary>
        public IReadOnlyList<string> Parts => _parts;
    }
}
