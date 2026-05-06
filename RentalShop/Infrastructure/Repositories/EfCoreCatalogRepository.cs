using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RentalShop.Domain.Entities;
using RentalShop.Infrastructure.Persistence;

namespace RentalShop.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the 'RealSubject' in the Proxy GoF pattern — the authoritative
    /// catalog store backed by PostgreSQL via EF Core.
    ///
    /// Domain role: persistent catalog repository. Replaces
    /// <see cref="InMemoryCatalogRepository"/> in production; both satisfy
    /// <see cref="ICatalogRepository"/> so the <see cref="CachingCatalogRepository"/>
    /// proxy wraps either implementation transparently.
    /// </summary>
    public sealed class EfCoreCatalogRepository : ICatalogRepository
    {
        private readonly RentalDbContext _context;
        private readonly ILogger<EfCoreCatalogRepository> _logger;

        public EfCoreCatalogRepository(
            RentalDbContext context,
            ILogger<EfCoreCatalogRepository> logger)
        {
            _context = context;
            _logger  = logger;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Uses <c>AsNoTracking</c> so the returned entity is disconnected from the
        /// <see cref="RentalDbContext"/> instance. The <see cref="CachingCatalogRepository"/>
        /// Proxy stores this object in <c>IMemoryCache</c>; a tracked entity would hold a
        /// reference to the now-disposed scoped DbContext on every subsequent request.
        /// </remarks>
        public async Task<RentalItem?> FindAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Proxy] RealSubject (EF Core) lookup for {Sku}", sku);
            return await _context.RentalItems
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Sku == sku, ct);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Uses <c>AsNoTracking</c> for the same reason as <see cref="FindAsync"/> —
        /// these entities may be held by callers or the cache after the DbContext is disposed.
        /// </remarks>
        public async Task<IReadOnlyList<RentalItem>> GetAllAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Proxy] RealSubject (EF Core) loading full catalog");
            return await _context.RentalItems
                .AsNoTracking()
                .ToListAsync(ct);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Because the entity was loaded with <c>AsNoTracking</c>, EF Core has no original-values
        /// snapshot. A plain <c>Update(item)</c> would treat <c>item.Version</c> as both the new
        /// value AND the original, generating <c>UPDATE … WHERE Version = &lt;NEW&gt;</c> — which
        /// always finds 0 rows and raises <see cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException"/>.
        ///
        /// The fix: save the DB-sourced version first, generate a fresh token, attach the entry,
        /// then override <c>OriginalValues[Version]</c> with the saved value so EF Core generates
        /// <c>UPDATE … SET Version = &lt;NEW&gt; … WHERE Version = &lt;OLD&gt;</c>.
        /// </remarks>
        public Task CommitStateAsync(RentalItem item, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Proxy] RealSubject committing state for {Sku}", item.Sku);

            // 1. Preserve the version that came from the DB (the WHERE clause value).
            var originalVersion = item.Version;

            // 2. Mint a fresh token (the SET clause value).
            item.Version = Guid.NewGuid();

            // 3. Attach and mark all scalar properties as modified.
            var entry = _context.RentalItems.Update(item);

            // 4. Restore the original version so EF Core uses it in the WHERE clause.
            //    Without this, EF Core would use the new Guid as the original, matching
            //    0 rows in the DB and raising DbUpdateConcurrencyException on every write.
            entry.OriginalValues[nameof(RentalItem.Version)] = originalVersion;

            return _context.SaveChangesAsync(ct);
        }

        /// <inheritdoc/>
        public async Task AddAsync(RentalItem item, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Proxy] RealSubject inserting new item {Sku}", item.Sku);
            _context.RentalItems.Add(item);
            await _context.SaveChangesAsync(ct);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Uses <c>ExecuteDeleteAsync</c> (EF Core 7+) to issue a direct
        /// <c>DELETE FROM RentalItems WHERE Sku = @sku</c> without loading or
        /// tracking the entity first — consistent with the <c>AsNoTracking</c>
        /// strategy used throughout this repository.
        /// </remarks>
        public async Task DeleteAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogDebug("[Pattern: Proxy] RealSubject deleting item {Sku}", sku);
            var deleted = await _context.RentalItems
                .Where(i => i.Sku == sku)
                .ExecuteDeleteAsync(ct);
            if (deleted == 0)
                _logger.LogWarning("Delete: Item {Sku} not found in database", sku);
        }
    }
}
