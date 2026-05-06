using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RentalShop.Application.Services;

namespace RentalShop.Application.Facades
{
    /// <summary>
    /// Represents a 'SubSystem' in the Facade GoF pattern (subsystem #4).
    ///
    /// Domain role: receipt / contract subsystem — drives the
    /// <see cref="DocumentRenderer"/> Template-Method document generator.
    /// </summary>
    public class DocumentService
    {
        private DocumentRenderer _renderer;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(DocumentRenderer renderer, ILogger<DocumentService> logger)
        {
            _renderer = renderer;
            _logger = logger;
        }

        /// <summary>
        /// Pick the document template at runtime — receipt for a return,
        /// contract for a fresh rental.
        /// </summary>
        public void UseRenderer(DocumentRenderer renderer) => _renderer = renderer;

        /// <summary>Emit the receipt / contract via the active renderer.</summary>
        public Task GenerateAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("Document generated: {DocumentType}", _renderer.GetType().Name);
            _renderer.Render();
            return Task.CompletedTask;
        }
    }
}
