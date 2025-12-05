using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class InvoiceDetail : BaseEntity
    {
        public string InvoiceId { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? VoucherId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalDiscount { get; set; } = 0;
        public virtual Invoice Invoice { get; set; } = default!;
        public virtual Book Book { get; set; } = default!;
        public virtual Voucher? Voucher { get; set; } 
    }
}
