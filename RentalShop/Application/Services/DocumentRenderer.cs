using Microsoft.Extensions.Logging;

namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents the 'AbstractClass' in the Template Method GoF pattern —
    /// defines the skeleton of an algorithm and defers the variable steps to
    /// subclasses.
    ///
    /// Domain role: fixed algorithm for producing a rental document
    /// (header → body → footer). Concrete subclasses fill in the variable
    /// steps to produce a "receipt" or a "rental contract"; the skeleton
    /// itself never changes.
    /// </summary>
    public abstract class DocumentRenderer
    {
        /// <summary>
        /// Logger shared with concrete subclasses. All document output goes
        /// through structured logging rather than direct console writes.
        /// </summary>
        protected readonly ILogger Logger;

        protected DocumentRenderer(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Variable step — concrete renderer decides what goes into the
        /// header (shop logo + receipt number, contract header + parties, …).
        /// </summary>
        public abstract void RenderHeader();

        /// <summary>
        /// Variable step — concrete renderer decides what footer / signature
        /// block to render.
        /// </summary>
        public abstract void RenderFooter();

        /// <summary>
        /// The Template Method itself. Locked-in document-generation flow;
        /// every rental document goes header → body → footer.
        /// </summary>
        public void Render()
        {
            RenderHeader();
            RenderBody();
            RenderFooter();
        }

        /// <summary>
        /// Fixed body block (line items / totals) that is identical for
        /// every document type.
        /// </summary>
        protected virtual void RenderBody()
        {
            Logger.LogInformation("──────────────────────────────");
            Logger.LogInformation(" Items & extras as ordered");
            Logger.LogInformation("──────────────────────────────");
        }
    }
}
