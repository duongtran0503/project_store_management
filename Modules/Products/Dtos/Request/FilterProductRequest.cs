using StoreManagement.API.Common.Responses;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Products.Dtos.Request
{
    public class FilterProductRequest :PaginationRequest
    {
        public string? SearchTerm { get; set; }
        public string? AuthorName { get; set; }
        public string? CategoryId { get; set; }

        [Range(0, (double)decimal.MaxValue)]
        public decimal? MinPrice { get; set; }
        [Range(0, (double)decimal.MaxValue)]
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; } = "CreatedAt"; 
        [RegularExpression("^(asc|desc)$", ErrorMessage = "Giá trị SortOrder phải là 'asc' hoặc 'desc'.")]
        public string? SortOrder { get; set; } 
    }
}
