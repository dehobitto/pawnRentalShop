using System;

namespace RentalShop.Models
{
    public class CatalogItemViewModel
    {
        public string    Sku                { get; init; } = string.Empty;
        public string    Name               { get; init; } = string.Empty;
        public string    Category           { get; init; } = string.Empty;
        public decimal   PricePerDay        { get; init; }
        public string    StateName          { get; init; } = string.Empty;
        public bool      IsAvailable        { get; init; }
        public bool      IsRented           { get; init; }
        public DateTime? ExpectedReturnDate { get; init; }
    }
}
