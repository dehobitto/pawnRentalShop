namespace RentalShop.Domain.Entities
{
    /// <summary>
    /// Represents a 'ConcreteProduct' in the Factory Method GoF pattern (variant A).
    ///
    /// Domain role: a power-tool rental item (drills, saws, etc.).
    /// </summary>
    public class ToolItem : RentalItem
    {
        public ToolItem(string sku, string name, decimal basePricePerDay)
        {
            Sku = sku;
            Name = name;
            BasePricePerDay = basePricePerDay;
        }
    }
}
