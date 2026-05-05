using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Facades
{
    /// <summary>
    /// Represents a 'SubSystem' in the Facade GoF pattern (subsystem #3).
    ///
    /// Domain role: payment / deposit subsystem — captures funds and records
    /// the transaction. Async because a real implementation calls an external
    /// payment gateway over the network.
    /// </summary>
    public class PaymentService
    {
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ILogger<PaymentService> logger)
        {
            _logger = logger;
        }

        /// <summary>Charge / deposit (mock). Returns a completed task; an EF/gateway impl would await.</summary>
        public Task CaptureAsync(decimal amount, CancellationToken ct = default)
        {
            _logger.LogInformation("Payment captured: €{Amount}", amount);
            return Task.CompletedTask;
        }
    }
}
