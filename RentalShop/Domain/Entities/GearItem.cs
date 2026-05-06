namespace RentalShop.Domain.Entities
{
    /// <summary>
    /// Represents a 'ConcreteProduct' in the Factory Method GoF pattern (variant B).
    ///
    /// Domain role: an outdoor-gear rental item (tents, kayaks, etc.).
    /// </summary>
    public class GearItem : RentalItem
    {
        public GearItem(string sku, string name, decimal basePricePerDay)
        {
            Sku = sku;
            Name = name;
            BasePricePerDay = basePricePerDay;
        }
    }
}
