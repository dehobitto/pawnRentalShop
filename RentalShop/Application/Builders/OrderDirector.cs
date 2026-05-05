namespace RentalShop.Application.Builders
{
    /// <summary>
    /// Represents the 'Director' in the Builder GoF pattern — drives the
    /// builder through the canonical construction sequence.
    ///
    /// Domain role: orchestrates the step-by-step assembly of any rental
    /// order (line items first, then extras), letting the concrete
    /// <see cref="OrderBuilder"/> decide what each step actually contributes.
    /// </summary>
    public class OrderDirector
    {
        /// <summary>
        /// Fixed assembly script — calls each builder step in the canonical
        /// order the shop uses.
        /// </summary>
        public void Build(OrderBuilder builder)
        {
            builder.AddItems();
            builder.AddExtras();
        }
    }
}
