using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RentalShop.Domain.Entities;
using RentalShop.Models;

namespace RentalShop.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the 'Proxy' in the Proxy GoF pattern — a surrogate that
    /// controls access to a real subject (here, by caching).
    ///
    /// Domain role: caching wrapper around the inner <see cref="ICatalogRepository"/>.
    /// Avoids repeated DB round-trips for the same SKU across HTTP requests.
    ///
    /// Lifetime note: registered as <b>Scoped</b> but the cache state lives in
    /// the injected <see cref="IMemoryCache"/> (application-wide cache), so entries
    /// survive across requests and are shared by all concurrent scopes.
    ///
    /// Cache policy: individual SKU entries are primed on <see cref="FindAsync"/>
    /// with a sliding expiration configured via
    /// <see cref="RentalShopSettings.CacheSlidingExpirationMinutes"/> in
    /// <c>appsettings.json</c>. They are explicitly invalidated on
    /// <see cref="CommitStateAsync"/>, <see cref="AddAsync"/>, and
    /// <see cref="DeleteAsync"/> so the next read always reflects the latest
    /// persisted state. The full item list is never cached because its
    /// invalidation surface is too large to track cheaply.
    /// </summary>
    public class CachingCatalogRepository : ICatalogRepository
    {
        private readonly ICatalogRepository _innerRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachingCatalogRepository> _logger;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public CachingCatalogRepository(
            ICatalogRepository innerRepository,
            IMemoryCache cache,
            IOptions<RentalShopSettings> options,
            ILogger<CachingCatalogRepository> logger)
        {
            _innerRepository = innerRepository;
            _cache           = cache;
            _logger          = logger;
            _cacheOptions    = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(
                    TimeSpan.FromMinutes(options.Value.CacheSlidingExpirationMinutes));
        }

        /// <inheritdoc/>
        public async Task<RentalItem?> FindAsync(string sku, CancellationToken ct = default)
        {
            if (_cache.TryGetValue<RentalItem>(sku, out var cached))
            {
                _logger.LogDebug("[Pattern: Proxy] Cache HIT for {Sku}", sku);
                return cached;
            }

            _logger.LogDebug("[Pattern: Proxy] Cache MISS for {Sku} — delegating to RealSubject", sku);
            var item = await _innerRepository.FindAsync(sku, ct);
            if (item is not null) _cache.Set(sku, item, _cacheOptions);
            return item;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyList<RentalItem>> GetAllAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Proxy] GetAllAsync — delegating to RealSubject (full list not cached)");
            return _innerRepository.GetAllAsync(ct);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Invalidates the per-SKU cache entry so the next <see cref="FindAsync"/>
        /// re-fetches the freshly persisted state rather than serving a stale snapshot.
        /// </remarks>
        public Task CommitStateAsync(RentalItem item, CancellationToken ct = default)
        {
            _cache.Remove(item.Sku);
            _logger.LogDebug("[Pattern: Proxy] Cache INVALIDATED for {Sku} after state commit", item.Sku);
            return _innerRepository.CommitStateAsync(item, ct);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Invalidates any existing cache entry for the SKU so the next
        /// <see cref="FindAsync"/> re-primes from the freshly inserted DB row.
        /// </remarks>
        public async Task AddAsync(RentalItem item, CancellationToken ct = default)
        {
            await _innerRepository.AddAsync(item, ct);
            _cache.Remove(item.Sku);
            _logger.LogDebug("[Pattern: Proxy] Cache INVALIDATED for new item {Sku} — will re-prime on next read", item.Sku);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Removes the cache entry before delegating so that a concurrent
        /// <see cref="FindAsync"/> cannot serve a stale entry for a deleted item.
        /// </remarks>
        public Task DeleteAsync(string sku, CancellationToken ct = default)
        {
            _cache.Remove(sku);
            _logger.LogDebug("[Pattern: Proxy] Cache INVALIDATED for deleted item {Sku}", sku);
            return _innerRepository.DeleteAsync(sku, ct);
        }
    }
}
