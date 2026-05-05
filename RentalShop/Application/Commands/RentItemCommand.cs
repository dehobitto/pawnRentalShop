using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Commands
{
    /// <summary>
    /// Represents a 'ConcreteCommand' in the Command GoF pattern (variant A).
    ///
    /// Domain role: "Rent" cashier action. Encapsulates everything needed to
    /// hand an item to a customer so the operation can be queued or undone.
    /// </summary>
    public class RentItemCommand : CashierCommand
    {
        private readonly string _sku;
        private readonly ILogger<RentItemCommand> _logger;

        public RentItemCommand(CashierTerminal terminal, string sku, ILogger<RentItemCommand> logger)
            : base(terminal)
        {
            _sku = sku;
            _logger = logger;
        }

        public override async Task ExecuteAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("[Rent] Handing {Sku} to customer", _sku);
            await Terminal.CommitAsync(ct);
        }

        public override Task UndoAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("[Rent] UNDO — taking {Sku} back from customer", _sku);
            return Task.CompletedTask;
        }
    }
}
