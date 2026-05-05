using RentalShop.Domain.Entities;

namespace RentalShop.Application.Factories
{
    /// <summary>
    /// Represents the abstract 'Creator' in the Factory Method GoF pattern —
    /// declares the factory method but defers instantiation to subclasses.
    ///
    /// Domain role: base contract for catalog item factories. The shop
    /// onboards new rental categories by subclassing this factory rather than
    /// editing a central instantiation switch.
    /// </summary>
    public abstract class ItemFactory
    {
        /// <summary>
        /// The Factory Method itself. Concrete factories instantiate the
        /// right kind of rental entity (tool, gear, …) without leaking that
        /// decision to callers.
        /// </summary>
        public abstract RentalItem Create();
    }
}
