namespace StoreManagement.API.Shared.Entities
{
    public class Product : BaseEntity
    {
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public string CategoryId { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? ProfitMargin { get; set; }
        public string Unit { get; set; } = "pcs";
        public decimal? Weight { get; set; }
        public string? Dimensions { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool HasVariants { get; set; } = false;

        // Navigation Properties
        public virtual Category Category { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual Inventory Inventory { get; set; } = null!;
        public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
        public virtual ICollection<InventoryHistory> InventoryHistories { get; set; } = new List<InventoryHistory>();
    }
}
