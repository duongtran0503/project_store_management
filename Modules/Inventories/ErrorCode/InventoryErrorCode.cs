using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Modules.Users.ErrorCode;

namespace StoreManagement.API.Modules.Inventories.ErrorCode
{
    public class InventoryErrorCode : IErrorCode
    {
        public static readonly InventoryErrorCode OutOfStock = new(400, "Sản phẩm không đủ cung cấp", false);
        public static readonly InventoryErrorCode ProductNotInStorage = new(400, "Sản phẩm không có trong cữa hàng", false);
        public static readonly InventoryErrorCode StaffInventoryInValid = new(400,"Mã nhân viên nhập hàng không đúng",false);
        public static readonly InventoryErrorCode InventoryDetailNotNull = new(400, "Phiếu nhập phải có ít nhật 1 sản phẩm", false);
        public static readonly InventoryErrorCode ReceiptNotFound = new(400, "Phiếu nhập không tồn tại", false);
        public static readonly InventoryErrorCode NOTALLOWEDIT = new(400, "Phiếu nhập không được phép sửa", false);
        public static readonly InventoryErrorCode InvaliedSupplier = new(400, "Nhà cung cấp không hợp lệ", false);
        public static readonly InventoryErrorCode InventoryReceiptNotExisted = new(400, "Phiếu nhập không tồn tại", false);

        public int StatusCode { get; }
        public string Message { get; }
        public bool Success { get; }
        public InventoryErrorCode(string message)
        {
            StatusCode = 400;
            Message = message;
            Success = false;
        }
        private InventoryErrorCode(int statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }
    }
}
