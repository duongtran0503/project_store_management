using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class InvoiceDetail : BaseEntity
    {
        public string InvoiceId { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Invoice Invoice { get; set; } = default!;
        public virtual Book Book { get; set; } = default!;
    }
}
