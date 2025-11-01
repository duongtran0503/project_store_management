using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class InventoryReceipt : BaseEntity
    {
        public DateTime ReceiptDate { get; set; }
        public string ReceivingStaffId { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
        public virtual Account ReceivingStaff { get; set; } = default!;
        public virtual Supplier Supplier { get; set; } = default!;
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();
    }
}
