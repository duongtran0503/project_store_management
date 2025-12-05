using StoreManagement.API.Modules.Products.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.API.Modules.Products.Dtos.Response
{
    public class BookResponse
    {
       
        public string Id { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string Image { get; set; } = ProductConstants.PRODUCT_DEFAULT_IMAGE;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string AuthorId { get; set; }= string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string PublisherId {  get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public string Status {  get; set; } = string.Empty;
        public string Description { get; set; } =string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public int StockCanBeSold { get; set; } = 0;
        public string CategoryName { get; set; } = string.Empty;
        public decimal RetailPrice { get; set; }
        public AppliedVoucher ActiveVoucher { get; set; } = default!;
    }
}
