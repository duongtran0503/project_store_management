using StoreManagement.API.Modules.Products.Validation;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Products.Dtos.Request
{
    public class UpdatePublisherRequest
    {

        [Required(ErrorMessage = "Tên Nhà Xuất Bản là bắt buộc.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Tên phải từ 5 đến 200 ký tự.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã Nhà Xuất Bản (Code) là bắt buộc.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Mã phải từ 3 đến 50 ký tự.")]
        [RegularExpression("^[a-zA-Z0-9_-]*$", ErrorMessage = "Mã chỉ được chứa ký tự chữ, số, dấu gạch dưới và dấu gạch ngang.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã Nhà Xuất Bản (Address) là bắt buộc.")]
        [StringLength(300, ErrorMessage = "Địa chỉ không được vượt quá 300 ký tự.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc.")]
        [AllowedAuthorStatus]
        public string Status { get; set; } = string.Empty;
    }
}
