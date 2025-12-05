using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Modules.Products.ErrorCode;

namespace StoreManagement.API.Modules.Promotions.ErrrorCode
{
    public class VoucherErrorCode
   : IErrorCode
    {
        public static readonly VoucherErrorCode VourcherExisted = new(400, "Mã giảm giá đã tồn tại", false);
        public static readonly VoucherErrorCode VourcherNotExisted = new(400, "Mã giảm giá không tồn tại", false);
        public static readonly VoucherErrorCode VoucherCodeExisted = new(400, "Code mã giảm giá đã tồn tại", false);
        public static readonly VoucherErrorCode InvalidIdTargeted = new(400, "Một số ID sản phẩm đã bị xóa hoặc không tồn tại", false);

        public int StatusCode { get; }
        public string Message { get; }
        public bool Success { get; }
        public VoucherErrorCode( string message)
        {
            StatusCode = 400;
            Message = message;
            Success = false;
        }
        private VoucherErrorCode(int statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }
    }
}
