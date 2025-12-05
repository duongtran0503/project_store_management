using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Modules.Inventories.ErrorCode;

namespace StoreManagement.API.Modules.Orders.ErrorCode
{
    public class InvoiceErrorCode:IErrorCode
    {
        public static readonly InvoiceErrorCode OutOfStock = new(400, "Sản phẩm không đủ cung cấp", false);
        public static readonly InvoiceErrorCode ProductNotInStorage = new(400, "Sản phẩm không có trong cữa hàng", false);
        public static readonly InvoiceErrorCode StaffInventoryInValid = new(400, "Mã nhân viên không đúng", false);
        public static readonly InvoiceErrorCode InvoiceDetailNotNull = new(400, "Đơn hàng phải có ít nhật 1 sản phẩm", false);
        public static readonly InvoiceErrorCode CustomerInventoryInValid = new(400, "Mã Khách hàng không đúng", false);

        public int StatusCode { get; }
        public string Message { get; }
        public bool Success { get; }
        public InvoiceErrorCode(string message)
        {
            StatusCode = 400;
            Message = message;
            Success = false;
        }
        private InvoiceErrorCode(int statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }
    }
}
