using StoreManagement.API.Modules.Orders.Constants;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Orders.Dtos.Requests
{
    public class CreateInvoiceRequest:IValidatableObject
    {
        [Required(ErrorMessage = "Loại đơn hàng (OrderType) là bắt buộc.")]
        [RegularExpression("^(ONLINE|POS)$", ErrorMessage = "Loại đơn hàng phải là 'ONLINE' hoặc 'POS'.")]
        public string OrderType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phương thức thanh toán là bắt buộc.")]
        public string PaymentMethod { get; set; } = string.Empty;


        public string PaymentStatus { get; set; } = string.Empty;

        public string? CustomerId { get; set; }

        public string Status { get; set; } =string.Empty;

        public decimal DiscountAmount { get; set; } = 0;

        public string? VoucherId { get; set; }


        [Range(0, double.MaxValue, ErrorMessage = "Số tiền thanh toán phải lớn hơn hoặc bằng 0.")]
        public decimal AmountPaid { get; set; } = 0;

        public string PaymentNote { get; set; } =string.Empty;

     
        [Required(ErrorMessage = "Chi tiết đơn hàng là bắt buộc.")]
        [MinLength(1, ErrorMessage = "Đơn hàng phải có ít nhất một sản phẩm.")]
        public List<InvoiceDetailDto> Details { get; set; } = new List<InvoiceDetailDto>();


      

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if(!InvoicePaymentStatusConstant.GetStrings().Contains(PaymentStatus))
            {
                results.Add(new ValidationResult("Trạng thái thành toán không hợp lệ"));
            }

            if(!InvoiceStatusConstant.GetStrings().Contains(Status))
            {
                results.Add(new ValidationResult("Trạng thái đơn hàng không hợp lệ"));
            }

            if (OrderType == InvoiceOrderTypeConstant.POS)
            {
              
               

               
                if (PaymentMethod == InvoicePaymentMothodConstant.CASH && AmountPaid == 0)
                {
                    results.Add(new ValidationResult("AmountPaid là bắt buộc khi thanh toán bằng Tiền mặt.", new[] { nameof(AmountPaid) }));
                }
            }
   
            else if (OrderType == InvoiceOrderTypeConstant.ONLINE)
            {
               
              

                
                if (PaymentMethod != InvoicePaymentMothodConstant.CASH && PaymentMethod !=InvoicePaymentMothodConstant.TRANSFER)
                {
                    results.Add(new ValidationResult("Phương thức thanh toán cho đơn Online phải là tiền mặt hoặc chuyển khoản.", new[] { nameof(PaymentMethod) }));
                }
            }

          

            return results;
        }
}
}
