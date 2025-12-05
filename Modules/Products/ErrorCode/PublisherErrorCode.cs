using StoreManagement.API.Common.Exceptions;

namespace StoreManagement.API.Modules.Products.ErrorCode
{
    public class PublisherErrorCode : IErrorCode
    {
        public static readonly PublisherErrorCode PublisherExisted = new(400, "Nhà cung cấp đã tồn tại", false);
        public static readonly PublisherErrorCode PublisherNotExisted = new(400, "Nhà cung cấp không tồn tại", false);

        public int StatusCode { get; }
        public string Message { get; }
        public bool Success { get; }

        private PublisherErrorCode(int statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }
    }
}
