using StoreManagement.API.Common.Exceptions;

namespace StoreManagement.API.Modules.Products.ErrorCode
{
    public class BookErrorCode :IErrorCode
    {
        public static readonly BookErrorCode BookExisted = new(400, "Sách đã tồn tại", false);
        public static readonly BookErrorCode BookNotExisted = new(400, "Sách không tồn tại", false);
        public int StatusCode { get; }
        public string Message { get; }
        public bool Success { get; }

        private BookErrorCode(int statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }
    }
}
