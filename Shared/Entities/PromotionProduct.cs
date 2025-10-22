namespace StoreManagement.API.Shared.Entities
{
    public class PromotionProduct : BaseEntity
    {
        public string PromotionId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;

        // Navigation Properties
        public virtual Promotion Promotion { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
