using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RentalShop.Domain.Entities;

namespace RentalShop.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the 'RealSubject' in the Proxy GoF pattern — the concrete
    /// resource the proxy stands in for.
    ///
    /// Domain role: the mock in-memory rental catalog repository. Stands in
    /// for what would be a database in production. Treated as the canonical
    /// source of truth for catalog data.
    /// </summary>
    public class InMemoryCatalogRepository : ICatalogRepository
    {
        private readonly Dictionary<string, RentalItem> _store = new();
        private readonly ILogger<InMemoryCatalogRepository> _logger;

        public InMemoryCatalogRepository(ILogger<InMemoryCatalogRepository> logger)
        {
            _logger = logger;
        }

        /// <summary>Adds an item to the mock store (used during catalog seeding).</summary>
        public void Seed(RentalItem item) => _store[item.Sku] = item;

        /// <inheritdoc/>
        public Task<RentalItem?> FindAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Proxy] RealSubject lookup for {Sku}", sku);
            return Task.FromResult(_store.TryGetValue(sku, out var item) ? item : null);
        }

        /// <inheritdoc/>
        public Task<IReadOnlyList<RentalItem>> GetAllAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Proxy] RealSubject returning full in-memory catalog");
            return Task.FromResult<IReadOnlyList<RentalItem>>(_store.Values.ToList());
        }

        /// <inheritdoc/>
        public Task CommitStateAsync(RentalItem item, CancellationToken ct = default)
        {
            // In-memory store holds object references — state change is already visible.
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task AddAsync(RentalItem item, CancellationToken ct = default)
        {
            _store[item.Sku] = item;
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(string sku, CancellationToken ct = default)
        {
            _store.Remove(sku);
            return Task.CompletedTask;
        }
    }
}
