using StoreManagement.API.Common.Responses;

namespace StoreManagement.API.Modules.Users.Dtos.Requests
{
    public class FilterUserRequest:PaginationRequest
    {
        public string? Phone {  get; set; } =string.Empty;
    }
}
