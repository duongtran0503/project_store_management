

namespace StoreManagement.API.Modules.Users.Dtos.Response
{
    public class UserResponse
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime? CreateAt { get; set; }
    }
}
