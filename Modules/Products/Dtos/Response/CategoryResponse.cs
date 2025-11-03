namespace StoreManagement.API.Modules.Products.Dtos.Response
{
    public class CategoryResponse
    {
        public required string CategoryName { get; set; }
        public required string Id { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
