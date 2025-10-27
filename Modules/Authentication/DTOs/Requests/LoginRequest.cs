using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Authentication.DTOs.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email quá dài")]
        public string Email { get; set; } =string.Empty;

        [Required(ErrorMessage = "Password không được để trống")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password phải có ít nhất 8 ký tự")]

        public String Password { get; set; }=string.Empty;
    }
}
