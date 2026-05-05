using RentalShop.Domain.Entities;

namespace RentalShop.Application.Builders
{
    /// <summary>
    /// Represents the abstract 'Builder' in the Builder GoF pattern —
    /// declares the step-by-step construction interface.
    ///
    /// Domain role: contract for rental-order assemblers. Different builders
    /// produce different order flavours (standard, premium, corporate) while
    /// the <see cref="OrderDirector"/>'s script stays untouched.
    /// </summary>
    public abstract class OrderBuilder
    {
        /// <summary>First assembly step — attaches the line items / rented goods.</summary>
        public abstract void AddItems();

        /// <summary>Second assembly step — attaches the extras (insurance, deposit, delivery, …).</summary>
        public abstract void AddExtras();

        /// <summary>Hands the finished order back to the caller.</summary>
        public abstract RentalOrder GetOrder();
    }
}
