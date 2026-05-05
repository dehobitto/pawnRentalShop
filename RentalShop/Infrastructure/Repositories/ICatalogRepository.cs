using System.Threading;
using System.Threading.Tasks;
using RentalShop.Domain.Entities;

namespace RentalShop.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the 'Subject' in the Proxy GoF pattern — common interface
    /// shared by the real subject and its proxy.
    ///
    /// Domain role: catalog-lookup contract. Both the in-memory repository
    /// and the caching wrapper implement it, so callers (notably the facade)
    /// never know whether a hit came from cache or store.
    /// </summary>
    public interface ICatalogRepository
    {
        /// <summary>
        /// Fetch a rental entity by its SKU asynchronously. The caching proxy
        /// short-circuits repeat calls; the real repository performs the actual
        /// lookup. NOTE: extended from the canonical parameterless
        /// <c>Request()</c> so the proxy can do real catalog work.
        /// </summary>
        Task<RentalItem?> FindAsync(string sku, CancellationToken ct = default);
    }
}
