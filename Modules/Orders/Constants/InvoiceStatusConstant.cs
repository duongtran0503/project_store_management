namespace StoreManagement.API.Modules.Orders.Constants
{
    public class InvoiceStatusConstant
    {
        public static readonly string PENDING = "PENDING";
        public static readonly string SHIPPED = "SHIPPED";
        public static readonly string DELIVERED = "DELIVERED";
        public static readonly string CANCELLED = "CANCELLED";

        public static string[] GetStrings()
        {
            return new string[] {
                PENDING,
                SHIPPED,
                DELIVERED,
                CANCELLED
            };

        }
    }
}
