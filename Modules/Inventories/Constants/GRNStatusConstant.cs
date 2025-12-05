namespace StoreManagement.API.Modules.Inventories.Constants
{
    public class GRNStatusConstant
    {
        public static string PENDING = "PENDING";
        public static string DRAFT = "DRAFT";
        public static string PROGRESS = "PROGRESS";
        public static string COMPLETED = "COMPLETED";
        public static string CANCELLED = "CANCELLED";

        public static string[] GetStrings() => new string[] {
            PENDING, DRAFT, PROGRESS, COMPLETED ,CANCELLED
        };


    }
}
