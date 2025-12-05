using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class Voucher : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public Decimal MinOrderValue { get; set; }
        public decimal MaxDiscountValue { get; set; }
        public int UsageCount { get; set; } = 1;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

        public ICollection<VoucherTarget> Targets { get; set; } = new List<VoucherTarget>();
    }
}
