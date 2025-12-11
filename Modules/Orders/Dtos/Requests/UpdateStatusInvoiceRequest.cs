using StoreManagement.API.Modules.Orders.Constants;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Orders.Dtos.Requests
{
    public class UpdateStatusInvoiceRequest:IValidatableObject
    {
        public string Status { get; set; } =string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!InvoiceStatusConstant.GetStrings().Contains(Status))
            {
                results.Add(new ValidationResult("Trạng thái đơn hàng không hợp lệ"));
            }
            if (!InvoicePaymentStatusConstant.GetStrings().Contains(PaymentStatus))
            {
                results.Add(new ValidationResult("Trạng thái thành toán không hợp lệ"));
            }

            return results;
        }
    }
}
