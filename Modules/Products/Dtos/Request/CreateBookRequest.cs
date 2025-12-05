using StoreManagement.API.Modules.Products.Validation;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Products.Dtos.Request
{
    public class CreateBookRequest
    {
        [Required(ErrorMessage = "Tiêu đề sách là bắt buộc.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Tiêu đề phải từ 5 đến 250 ký tự.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "ID tác giả là bắt buộc.")]

        public string AuthorId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Id nhà xuất bản là bắt buộc.")]
  
        public string PublisherId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã ISBN là bắt buộc.")]
        [StringLength(20, ErrorMessage = "Mã ISBN không hợp lệ.")]
     
        public string Isbn { get; set; } = string.Empty;

        [Required(ErrorMessage = "ID danh mục là bắt buộc.")]
        public string CategoryId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá bán lẻ là bắt buộc.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Giá bán lẻ phải lớn hơn 0.")]
        public decimal RetailPrice { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc.")]
        [AllowedAuthorStatus]
        public string Status { get; set; } = string.Empty;

        public string? Image { get; set; }
    }
}
