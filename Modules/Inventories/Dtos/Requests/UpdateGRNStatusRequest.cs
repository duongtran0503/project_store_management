using StoreManagement.API.Modules.Inventories.Constants;
using StoreManagement.API.Modules.Orders.Constants;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Inventories.Dtos.Requests
{
    public class UpdateGRNStatusRequest : IValidatableObject
    {
        public string Status { get; set; } = string.Empty;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!GRNStatusConstant.GetStrings().Contains(Status))
            {
                results.Add(new ValidationResult("Trạng thái phiếu nhập không hợp lệ"));
            }
            return results;
        }
    }
}
