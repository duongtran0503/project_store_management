using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Products.Dtos.Request
{
    public class CreateBookRequest
    {
        [Required(ErrorMessage = "Tiêu đề sách là bắt buộc.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Tiêu đề phải từ 5 đến 250 ký tự.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên tác giả là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được quá 100 ký tự.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nhà xuất bản là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên nhà xuất bản không được quá 100 ký tự.")]
        public string Publisher { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã ISBN là bắt buộc.")]
        [StringLength(20, ErrorMessage = "Mã ISBN không hợp lệ.")]
     
        public string Isbn { get; set; } = string.Empty;

        [Required(ErrorMessage = "ID danh mục là bắt buộc.")]
        public string CategoryId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá bán lẻ là bắt buộc.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Giá bán lẻ phải lớn hơn 0.")]
        public decimal RetailPrice { get; set; }

        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải là số nguyên không âm.")]
        public int StockQuantity { get; set; }

        public string? Image { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
