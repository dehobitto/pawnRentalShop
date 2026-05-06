using System;
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

        /// <summary>Charge / deposit (mock). Returns a completed task; a real impl would await the gateway.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="amount"/> is negative.</exception>
        public Task CaptureAsync(decimal amount, CancellationToken ct = default)
        {
            if (amount < decimal.Zero)
                throw new ArgumentOutOfRangeException(nameof(amount), amount,
                    "Payment amount must not be negative.");

            _logger.LogInformation("Payment captured: €{Amount}", amount);
            return Task.CompletedTask;
        }
    }
}
