using StoreManagement.API.Common.Responses;

namespace StoreManagement.API.Modules.Products.Dtos.Request
{
    public class FIlterPublisherRequest:PaginationRequest
    {
        public string? SearchTerm { get; set; }
    }
}
