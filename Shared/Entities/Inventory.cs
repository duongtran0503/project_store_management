namespace StoreManagement.API.Shared.Entities
{
    public class Inventory : BaseEntity
    {
        public string ProductId { get; set; } = string.Empty;
        public string? VariantId { get; set; }
        public int CurrentQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int MinimumStockLevel { get; set; } = 10;
        public int MaximumStockLevel { get; set; } = 100;
        public DateTime? LastRestockedDate { get; set; }

        // Navigation Properties
        public virtual Product Product { get; set; } = null!;
        public virtual ProductVariant? ProductVariant { get; set; }
    }
}
