namespace StoreManagement.API.Modules.Products.Validation
{
    using StoreManagement.API.Modules.Products.Constants;
    using System.ComponentModel.DataAnnotations;

    public class AllowedAuthorStatusAttribute : ValidationAttribute
    {
        private readonly HashSet<string> _allowedStatuses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        AuthorStatusConstants.DEFAULT,
        AuthorStatusConstants.INACTIVE
    };

       
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
               
                return ValidationResult.Success;
            }

            var status = value.ToString();

           
            if (!_allowedStatuses.Contains(status))
            {
               
                var errorMessage = $"Trạng thái không hợp lệ. Chỉ chấp nhận '{AuthorStatusConstants.DEFAULT}' hoặc '{AuthorStatusConstants.INACTIVE}'.";
                return new ValidationResult(errorMessage, new[] { validationContext.MemberName! });
            }
            return ValidationResult.Success;
        }
    }
}
