using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Suppliers.Dtos.Requests
{
    public class UpdateSupplierRequest
    {
        [Required(ErrorMessage = "Tên Nhà cung cấp là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Tên Nhà cung cấp không được vượt quá 200 ký tự.")]
        public string SupplierName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Tên Người liên hệ là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên Người liên hệ không được vượt quá 100 ký tự.")]
        public string ContactPerson { get; set; } = string.Empty;


        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ.")]
        public string Phone { get; set; } = string.Empty;


        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        [StringLength(300, ErrorMessage = "Địa chỉ không được vượt quá 300 ký tự.")]
        public string Address { get; set; } = string.Empty;
    }
}
