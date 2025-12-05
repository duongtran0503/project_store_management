namespace StoreManagement.API.Modules.Products.Dtos.Response
{
    public class PublisherResponse
    {
        public required string Name { get; set; }
        public required string Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Address { get; set; }  = string.Empty ;
        public string Status { get; set; } = string.Empty;
        public int TotalBook { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
