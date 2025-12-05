namespace StoreManagement.API.Modules.Orders.Constants
{
    public class InvoicePaymentStatusConstant
    {
        public static readonly string PAID = "PAID";
        public static readonly string UNPAID = "UNPAID";

        public static string[] GetStrings() => new string[] { PAID, UNPAID };
    }
}
