namespace StoreManagement.API.Shared.Entities
{
    public class Promotion : BaseEntity
    {
        public string PromotionCode { get; set; } = string.Empty;
        public string PromotionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DiscountType { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public decimal MinOrderAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public int CustomerUsageLimit { get; set; } = 1;
        public string ApplyFor { get; set; } = "all";
        public string Status { get; set; } = "active";
        public string CreatedById { get; set; } = string.Empty;

        // Navigation Properties
        public virtual User CreatedBy { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
    }
}
