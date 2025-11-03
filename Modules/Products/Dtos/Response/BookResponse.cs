namespace StoreManagement.API.Modules.Products.Dtos.Response
{
    public class BookResponse
    {
       
        public string Id { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string Image { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;

        public string CategoryId { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public decimal RetailPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
