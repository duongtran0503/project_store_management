namespace StoreManagement.API.Shared.Entities
{
    public class Order : BaseEntity
    {
        public string OrderCode { get; set; } = string.Empty;
        public string? CustomerId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? PromotionId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string OrderStatus { get; set; } = "pending";
        public string PaymentStatus { get; set; } = "pending";
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public string? CustomerNotes { get; set; }
        public string? InternalNotes { get; set; }

        // Navigation Properties
        public virtual Customer? Customer { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Promotion? Promotion { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
