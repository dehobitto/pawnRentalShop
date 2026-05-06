using System.Collections.Generic;

namespace RentalShop.Domain.Entities
{
    /// <summary>
    /// Represents the abstract 'Component' in the Composite GoF pattern —
    /// declares the common interface for both leaf nodes and composites.
    ///
    /// Domain role: base abstraction for any node in the rental package tree.
    /// <see cref="GetDisplayLines"/> is the 'Operation' in the pattern — it
    /// returns structured text lines so callers (controllers, loggers) can
    /// present the tree without the domain layer owning any I/O concern.
    ///
    /// Persistence note: mapped as Table-Per-Hierarchy (TPH) with a single
    /// <c>PackageComponents</c> table; <c>ComponentType</c> discriminator
    /// distinguishes <see cref="RentalLineItem"/> ("LineItem") from
    /// <see cref="RentalPackage"/> ("Package").
    /// </summary>
    public abstract class PackageComponent
    {
        private const int IndentWidth = 2;

        /// <summary>Surrogate primary key for EF Core; carries no domain meaning.</summary>
        public int Id { get; set; }

        /// <summary>Display name of this node (item name or bundle label).</summary>
        public string Name { get; private set; } = string.Empty;

        protected PackageComponent(string name) { Name = name; }

        /// <summary>Parameterless constructor required by EF Core for entity materialisation.</summary>
        protected PackageComponent() { }

        /// <summary>
        /// Returns the leading whitespace for a given tree depth.
        /// Used by <see cref="GetDisplayLines"/> implementations to produce
        /// consistent indentation without repeating the spacing formula.
        /// </summary>
        protected static string Indent(int depth) => new string(' ', depth * IndentWidth);

        /// <summary>
        /// Represents the 'Add' child-management operation in the Composite GoF pattern.
        /// No-op on leaf nodes.
        /// </summary>
        public abstract void Add(PackageComponent child);

        /// <summary>
        /// Represents the 'Remove' child-management operation in the Composite GoF pattern.
        /// No-op on leaf nodes.
        /// </summary>
        public abstract void Remove(PackageComponent child);

        /// <summary>
        /// Represents the 'Operation' in the Composite GoF pattern.
        /// Returns the tree as indented display lines. Both leaf and composite
        /// implement it uniformly so callers never need to distinguish the two.
        /// </summary>
        public abstract IEnumerable<string> GetDisplayLines(int depth = 0);

        /// <summary>
        /// Extended operation alongside the canonical pattern — returns the
        /// total rental price, recursively for composites.
        /// </summary>
        public abstract decimal GetPrice();
    }
}
