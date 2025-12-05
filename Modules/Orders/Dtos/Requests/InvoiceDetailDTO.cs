using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Orders.Dtos.Requests
{
    public class InvoiceDetailDto
    {
        [Required(ErrorMessage ="Thông tin sản phẩm là bắt buộc")]
        public string BookId { get; set; }

        [Required(ErrorMessage ="Số lượng sản phẩm là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
        public string? VoucherId { get; set; }

        [Required(ErrorMessage = "Giảm giá là bắt buộc")]
        [Range(minimum:0, double.MaxValue, ErrorMessage = "Giảm giá phải lớn hơn hoặc bàng 0.")]
        public decimal TotalDiscount { get; set; }
    }
}
