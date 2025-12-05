using StoreManagement.API.Common.Exceptions;

namespace StoreManagement.API.Modules.Users.ErrorCode
{
    public class CustomerErrorCode : IErrorCode
    {
        public static readonly CustomerErrorCode CustomerExisted = new(400, "Khách hàng đã tồn tại", false);
        public static readonly CustomerErrorCode CustomerNotExisted = new(400, "Khách hàng  không tồn tại", false);
      
        public int StatusCode { get; }
        public string Message { get; }
        public bool Success { get; }
        public CustomerErrorCode(string message)
        {
            StatusCode = 400;
            Message = message;
            Success = false;
        }
        private CustomerErrorCode(int statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }
    }
}
