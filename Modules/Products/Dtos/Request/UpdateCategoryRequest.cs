using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Products.Dtos.Request
{
    public class UpdateCategoryRequest
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự.")]
        public string CategoryName { get; set; } = string.Empty;

      
        [Required(ErrorMessage = "Mã code danh mục là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Mã code không được vượt quá 50 ký tự.")]
        public string CategoryCode { get; set; } = string.Empty;
    }
}
