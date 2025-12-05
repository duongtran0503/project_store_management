using StoreManagement.API.Common.Responses;

namespace StoreManagement.API.Modules.Products.Dtos.Request
{
    public class FiltertAuthorRequest:PaginationRequest
    {
        public string? SearchTerm { get; set; }
    }
}
