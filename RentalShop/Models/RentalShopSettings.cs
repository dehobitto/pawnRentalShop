namespace RentalShop.Models
{
    /// <summary>
    /// Strongly-typed settings bound from the <c>RentalShop</c> section of
    /// <c>appsettings.json</c>. Registered via
    /// <c>services.Configure&lt;RentalShopSettings&gt;()</c> and consumed
    /// through <c>IOptions&lt;RentalShopSettings&gt;</c> so values can be
    /// changed at deploy time without recompilation.
    /// </summary>
    public class RentalShopSettings
    {
        public const string Section = "RentalShop";

        /// <summary>Sliding cache TTL in minutes (default 5).</summary>
        public int     CacheSlidingExpirationMinutes { get; init; } = 5;

        /// <summary>Post-discount multiplier for loyalty customers (default 0.85 = 15% off).</summary>
        public decimal LoyaltyMultiplier             { get; init; } = 0.85m;

        /// <summary>Weekend surcharge multiplier (default 1.5 = +50%).</summary>
        public decimal WeekendMultiplier             { get; init; } = 1.5m;
    }
}
