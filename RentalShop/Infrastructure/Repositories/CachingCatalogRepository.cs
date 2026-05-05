using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RentalShop.Domain.Entities;

namespace RentalShop.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the 'Proxy' in the Proxy GoF pattern — a surrogate that
    /// controls access to a real subject (here, by caching).
    ///
    /// Domain role: caching wrapper around the inner <see cref="ICatalogRepository"/>.
    /// Avoids repeated catalog lookups for the same SKU within a session.
    /// Thread-safe: the cache uses <see cref="ConcurrentDictionary{TKey,TValue}"/>
    /// so concurrent HTTP requests cannot race on the same cache slot.
    /// </summary>
    public class CachingCatalogRepository : ICatalogRepository
    {
        private readonly ICatalogRepository _innerRepository;
        private readonly ConcurrentDictionary<string, RentalItem> _cache = new();
        private readonly ILogger<CachingCatalogRepository> _logger;

        public CachingCatalogRepository(
            ICatalogRepository innerRepository,
            ILogger<CachingCatalogRepository> logger)
        {
            _innerRepository = innerRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<RentalItem?> FindAsync(string sku, CancellationToken ct = default)
        {
            if (_cache.TryGetValue(sku, out var cached))
            {
                _logger.LogDebug("Cache HIT for {Sku}", sku);
                return cached;
            }

            _logger.LogDebug("Cache MISS for {Sku} — delegating to inner repository", sku);
            var item = await _innerRepository.FindAsync(sku, ct);
            if (item is not null) _cache[sku] = item;
            return item;
        }
    }
}
