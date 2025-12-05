using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class Invoice : BaseEntity
    {
        public string? CashierStaffId { get; set; }
        public string? CustomerId { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public string? VoucherId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentNote { get; set; }= string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal FinalAmount { get; set; }

        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public virtual Customer? Customer { get; set; }
        public virtual Account? CashierStaff { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }
}
