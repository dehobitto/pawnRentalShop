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
    /// itself never changes. In the web layer, subclasses will be replaced by
    /// Razor partial views; the pattern contract is preserved here.
    /// </summary>
    public abstract class DocumentRenderer
    {
        /// <summary>
        /// Shared structured logger. All document lifecycle output goes through
        /// structured logging — no console or string output from this layer.
        /// </summary>
        protected readonly ILogger Logger;

        protected DocumentRenderer(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Variable step — concrete renderer decides what goes into the header.
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
            Logger.LogDebug("[Pattern: Template Method] {DocumentType}.Render starting",
                GetType().Name);
            RenderHeader();
            RenderBody();
            RenderFooter();
            Logger.LogDebug("[Pattern: Template Method] {DocumentType}.Render complete",
                GetType().Name);
        }

        /// <summary>
        /// Fixed body block (line items / totals) — identical for every
        /// document type, so it is not overridable in subclasses.
        /// </summary>
        protected virtual void RenderBody()
        {
            Logger.LogDebug("[Pattern: Template Method] {DocumentType}.RenderBody (fixed step)",
                GetType().Name);
        }
    }
}
