using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Inventories.Dtos.Requests
{
    public class CreateReceiptDetailRequest
    {
        [Required]
        public string BookId { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng nhập phải lớn hơn 0.")]
        public int QuantityReceived { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá nhập phải lớn hơn 0.")]
        public decimal UnitCost { get; set; }
    }
}
