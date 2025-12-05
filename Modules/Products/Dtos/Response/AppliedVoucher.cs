namespace StoreManagement.API.Modules.Products.Dtos.Response
{
    public class AppliedVoucher
    {
        public string VoucherId { get; set; } =string.Empty;
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; } = 0;
        public decimal DiscountPrice { get; set; } = 0;    
        public string Type { get; set; } = string.Empty;
    }
}
