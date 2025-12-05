using StoreManagement.API.Common.Responses;

namespace StoreManagement.API.Modules.Promotions.Dtos.Requests
{
    public class FilterVoucherRequest:PaginationRequest
    {
        public string? SearchTerm { get; set; }
    }
}
