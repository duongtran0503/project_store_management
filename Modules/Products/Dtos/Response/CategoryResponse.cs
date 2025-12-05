namespace StoreManagement.API.Modules.Products.Dtos.Response
{
    public class CategoryResponse
    {
        public required string CategoryName { get; set; }
        public required string Id { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public string Status { get; set; } = string.Empty;
        public int TotalBooks { get; set; } = 0;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
