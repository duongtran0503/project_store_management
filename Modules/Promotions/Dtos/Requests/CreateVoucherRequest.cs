using StoreManagement.API.Modules.Promotions.Constants;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Modules.Promotions.Dtos.Requests
{
    public class CreateVoucherRequest : IValidatableObject
    {

        [Required(ErrorMessage = "Tên Voucher là bắt buộc.")]
        [MaxLength(255, ErrorMessage = "Tên không được vượt quá 255 ký tự.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mã Code là bắt buộc.")]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public decimal MinOrderValue { get; set; } = 0;
        public decimal MaxDiscountValue { get; set; } = 0;
        public int MaxUses { get; set; } = 1;

       
        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc.")]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public string Type { get; set; } 

        public List<string> TargetIds { get; set; } = new List<string>();

        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "Ngày kết thúc phải muộn hơn Ngày bắt đầu.",
                    new[] { nameof(EndDate), nameof(StartDate) });
            }
            var validTargets = new[] { VoucherTargetConstants.ProductTarget,
                VoucherTargetConstants.CategoryTarget };
            if (string.IsNullOrEmpty(Type))
            {
               
                yield return new ValidationResult(
                    "Loại mục tiêu áp dụng (ApplyTarget) là bắt buộc.",
                    new[] { nameof(Type) });
            }
            else if (!validTargets.Contains(Type, StringComparer.OrdinalIgnoreCase))
            {
                
                var validList = string.Join(", ", validTargets);

                yield return new ValidationResult(
                    $"Loại mục tiêu '{Type}' không hợp lệ. Chỉ chấp nhận các giá trị: {validList}.",
                    new[] { nameof(Type) });
            }
        }
    }
}
