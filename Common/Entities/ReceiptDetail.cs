using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class ReceiptDetail : BaseEntity
    {
        public string ReceiptId { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;
        public int QuantityReceived { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalLineCost { get; set; }
        public virtual InventoryReceipt Receipt { get; set; } = default!;
        public virtual Book Book { get; set; } = default!;
    }
}
