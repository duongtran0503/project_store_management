namespace StoreManagement.API.Shared.Entities
{
    public class InventoryHistory : BaseEntity
    {
        public string ProductId { get; set; } = string.Empty;
        public string? VariantId { get; set; }
        public string ChangeType { get; set; } = string.Empty;
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public int ChangeQuantity { get; set; }
        public string? ReferenceId { get; set; }
        public string? ReferenceType { get; set; }
        public string? Note { get; set; }
        public string CreatedById { get; set; } = string.Empty;

        // Navigation Properties
        public virtual Product Product { get; set; } = null!;
        public virtual ProductVariant? ProductVariant { get; set; }
        public virtual User CreatedBy { get; set; } = null!;
    }
}
