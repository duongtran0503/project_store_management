using StoreManagement.API.Common.Exceptions;

namespace StoreManagement.API.Modules.Users.ErrorCode
{
    public class UserErrorCode:IErrorCode
    {
        public static readonly UserErrorCode UserExisted = new(400, "Người dùng đã tồn tại", false);
        public static readonly UserErrorCode UserNotExisted = new(400, "Người dùng không tồn tại", false);

        public int StatusCode { get; }
        public string Message { get; }
        public bool Success { get; }
        public UserErrorCode(string message)
        {
            StatusCode = 400;
            Message = message;
            Success = false;
        }
        private UserErrorCode(int statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }
    }
}
