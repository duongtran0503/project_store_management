using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Users.Dtos.Requests
{
    public class CreateCustomerRequest
    {
       
        [Required(ErrorMessage = "Tên khách hàng là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Địa chỉ giao hàng là bắt buộc.")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự.")]
        public string Address { get; set; } = string.Empty; 


        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ.")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 20 ký tự.")]
        public string Phone { get; set; } = string.Empty;
    }
}
