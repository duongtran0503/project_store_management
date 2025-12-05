using StoreManagement.API.Common.Exceptions;

namespace StoreManagement.API.Modules.Products.ErrorCode
{
    public class AuthorErrorCode:IErrorCode
    {
        public static readonly AuthorErrorCode AuthorExisted = new(400, "Tác giả đã tồn tại", false);
        public static readonly AuthorErrorCode AuthorNotExisted = new(400, "Tác giả không tồn tại", false);

        public int StatusCode { get; }
        public string Message { get; }
        public bool Success { get; }

        private AuthorErrorCode(int statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }
    }
}
