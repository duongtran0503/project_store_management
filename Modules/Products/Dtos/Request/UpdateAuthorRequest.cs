using StoreManagement.API.Modules.Products.Validation;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Products.Dtos.Request
{
    public class UpdateAuthorRequest
    {
        [Required(ErrorMessage = "Tên tác giả là bắt buộc.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Tên tác giả phải từ 3 đến 150 ký tự.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã tác giả là bắt buộc.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Mã tác giả phải từ 3 đến 50 ký tự.")]
        [RegularExpression("^[a-zA-Z0-9_-]*$", ErrorMessage = "Mã tác giả chỉ được chứa ký tự chữ, số, dấu gạch dưới và dấu gạch ngang.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Trạng thái là bắt buộc.")]
        [AllowedAuthorStatus]
        public string Status { get; set; } = string.Empty;

    }
}
