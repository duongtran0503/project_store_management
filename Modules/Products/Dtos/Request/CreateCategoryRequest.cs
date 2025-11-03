using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Products.Dtos.Request
{
    public class CreateCategoryRequest

    {

        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên danh mục phải dài từ 3 đến 100 ký tự.")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Mã danh mục là bắt buộc.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Mã danh mục phải dài từ 3 đến 100 ký tự.")]
        public string CategoryCode { get; set; }
    }
}
