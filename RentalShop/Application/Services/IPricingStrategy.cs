namespace RentalShop.Application.Services
{
    /// <summary>
    /// Represents the 'Strategy' in the Strategy GoF pattern — common
    /// interface for the family of interchangeable algorithms.
    ///
    /// Domain role: contract for swappable rental-pricing rules. Adding a new
    /// pricing scheme means writing a new implementation, never editing
    /// existing ones.
    /// </summary>
    public interface IPricingStrategy
    {
        /// <summary>
        /// Calculates the rental price for the given base rate and number of
        /// days under this strategy's rules. NOTE: extended from the
        /// canonical parameterless <c>AlgorithmInterface()</c> so the rental
        /// domain can pass real inputs and receive a real total.
        /// </summary>
        decimal Calculate(decimal baseRate, int days);
    }
}
