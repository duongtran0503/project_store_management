namespace StoreManagement.API.Modules.Promotions.Dtos.Responses
{
    public class DetailVoucherResponse
    {
        public required string Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public decimal MinOrderValue { get; set; } = 0;
        public decimal MaxDiscountValue { get; set; } = 0;
        public int MaxUses { get; set; } = 1;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string ApplyTarget { get; set; }
        public List<VoucherItemTargetResponse> TargetDetail { get; set; } = new List<VoucherItemTargetResponse>();
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
