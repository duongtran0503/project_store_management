namespace StoreManagement.API.Shared.Entities
{
    public class Payment : BaseEntity
    {
        public string OrderId { get; set; } = string.Empty;
        public string PaymentCode { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "cash";
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string? ReferenceNumber { get; set; }
        public string PaymentStatus { get; set; } = "completed";
        public string? Notes { get; set; }
        public string CreatedById { get; set; } = string.Empty;

        // Navigation Properties
        public virtual Order Order { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
    }
}
