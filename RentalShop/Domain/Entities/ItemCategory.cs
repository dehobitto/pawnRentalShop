namespace RentalShop.Domain.Entities
{
    /// <summary>
    /// Rental item categories understood by concrete item factory creators.
    /// Replaces the "Gear" / "Tool" magic strings used in facade and view model.
    /// </summary>
    public enum ItemCategory { Tool, Gear }
}
