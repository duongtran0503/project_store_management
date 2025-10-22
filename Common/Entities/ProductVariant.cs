namespace StoreManagement.API.Shared.Entities
{
    public class ProductVariant : BaseEntity
    {
        public string ProductId { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Material { get; set; }
        public decimal AdditionalPrice { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual Product Product { get; set; } = null!;
        public virtual Inventory Inventory { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<InventoryHistory> InventoryHistories { get; set; } = new List<InventoryHistory>();
    }
}
