using StoreManagement.API.Common.Exceptions;


namespace StoreManagement.API.Modules.Products.ErrorCode
{
    public class CategoryErrorCode:IErrorCode
    {
        public static readonly CategoryErrorCode CategoryExisted = new(400, "Danh mục đã tồn tại", false);
        public static readonly CategoryErrorCode CategoryCodeExisted = new(400, "Mã danh mục đã tồn tại", false);
        public static readonly CategoryErrorCode CategoryNotExisted = new(400, "Danh mục không tồn tại", false);
        public static readonly CategoryErrorCode CategoryCantNotDelete = new(400, "Dạnh mục đang còn sản phẩm không thể xóa được", false);
        public int StatusCode { get; }
        public string Message { get; }
        public bool Success { get; }

        private CategoryErrorCode(int statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }
    }
}
