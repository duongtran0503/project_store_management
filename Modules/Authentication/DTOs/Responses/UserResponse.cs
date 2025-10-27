using StoreManagement.API.Modules.Authentication.Constants;

namespace StoreManagement.API.Modules.Authentication.DTOs.Responses
{
    public class UserResponse
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = Roles.STAFF.ToString();
        public bool IsActive { get; set; } = true;
        public DateTime? CreateAt { get; set; }
    }
}
