using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Authentication.DTOs.Requests
{

    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "UserName không được để trống")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "UserName phải từ 3 đến 30 ký tự")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password không được để trống")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password phải có ít nhất 8 ký tự")]
      
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email quá dài")]
        public string Email { get; set; } = string.Empty;
    }
}
