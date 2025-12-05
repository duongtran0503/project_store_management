using StoreManagement.API.Common.Responses;

namespace StoreManagement.API.Modules.Suppliers.Dtos.Requests
{
    public class SearchSupplierRequest:PaginationRequest
    {
        public string? Query {  get; set; }
    }
}
