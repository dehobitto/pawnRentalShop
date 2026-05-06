namespace RentalShop.Models
{
    /// <summary>Business-rule constants shared across view models and validation attributes.</summary>
    public static class RentalConstants
    {
        public const int    MinRentalDays = 1;
        public const int    MaxRentalDays = 365;
        public const double MinItemPrice  = 0.01;
        public const double MaxItemPrice  = 9999.99;
    }
}
