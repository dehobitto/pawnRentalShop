namespace RentalShop.Application.Services
{
    /// <summary>
    /// Value object that groups the two <see cref="DocumentRenderer"/> instances
    /// required by the <c>RentalShopFacade</c>.
    ///
    /// Replaces two consecutive loose constructor parameters that were trivially
    /// swappable at the call site; the named properties make every assignment
    /// self-documenting.
    /// </summary>
    public record DocumentRenderers(DocumentRenderer Contract, DocumentRenderer Receipt);
}
