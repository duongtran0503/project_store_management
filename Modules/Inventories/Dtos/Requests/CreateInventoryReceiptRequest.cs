using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Inventories.Dtos.Requests
{
    public class CreateInventoryReceiptRequest
    {

        [Required(ErrorMessage = "Mã nhà cung cấp là bắt buộc.")]
        public string SupplierId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chi tiết nhập hàng là bắt buộc.")]
        [MinLength(1, ErrorMessage = "Phiếu nhập phải có ít nhất một sản phẩm.")]
        public List<CreateReceiptDetailRequest> Details { get; set; } = new List<CreateReceiptDetailRequest>();
    }
}
