namespace StoreManagement.API.Shared.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public string PoCode { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;
        public DateTime PoDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; } = "draft";
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public string CreatedById { get; set; } = string.Empty;

        // Navigation Properties
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    }
}
