using Microsoft.AspNetCore.Http;
using StoreManagement.API.Common.Exceptions;


namespace StoreManagement.API.Modules.Suppliers.ErrorCode
{
    public class SupplierErrorCode :IErrorCode
    {
        public static readonly SupplierErrorCode SupplierNotExsisted = new(400, "Nhà cung cấp không tồn tại", false);
        public static readonly SupplierErrorCode SupplierExsisted = new(400, "Nhà cung cấp đã tồn tại", false);
        public static readonly SupplierErrorCode SupplierPhoneExsisted = new(400, "Số điện thoại nhà cung cấp đã tồn tại", false);

        public int StatusCode { get; }
    public string Message { get; }
    public bool Success { get; }

    private SupplierErrorCode(int statusCode, string message, bool success)
    {
        StatusCode = statusCode;
        Message = message;
        Success = success;
    }
   }
}
