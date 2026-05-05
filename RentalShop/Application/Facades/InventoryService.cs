using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RentalShop.Domain.Entities;
using RentalShop.Infrastructure.Repositories;

namespace RentalShop.Application.Facades
{
    /// <summary>
    /// Represents a 'SubSystem' in the Facade GoF pattern (subsystem #1).
    ///
    /// Domain role: inventory subsystem — checks item availability via the
    /// caching <see cref="ICatalogRepository"/> and "reserves" stock. Hidden
    /// from the UI by the <see cref="RentalShopFacade"/>.
    /// </summary>
    public class InventoryService
    {
        private readonly ICatalogRepository _catalog;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(ICatalogRepository catalog, ILogger<InventoryService> logger)
        {
            _catalog = catalog;
            _logger = logger;
        }

        /// <summary>
        /// Look the SKU up in the catalog (cache-first via the proxy) and
        /// report whether it can be reserved.
        /// </summary>
        public async Task<RentalItem?> ReserveAsync(string sku, CancellationToken ct = default)
        {
            _logger.LogInformation("Reserving {Sku}", sku);
            var item = await _catalog.FindAsync(sku, ct);
            if (item is null)
            {
                _logger.LogWarning("{Sku} not found in catalog — aborting reservation", sku);
                return null;
            }
            _logger.LogInformation("Reserved: {Item}", item);
            return item;
        }
    }
}
