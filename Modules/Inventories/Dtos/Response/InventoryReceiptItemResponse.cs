namespace StoreManagement.API.Modules.Inventories.Dtos.Response
{
    public class InventoryReceiptItemResponse
    {
      public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int ImportQuantity { get; set; } = 0;
   
        public decimal ImportPrice { get; set; } = decimal.Zero;

        public decimal TotalValueImport { get; set; } = decimal.Zero;

        public string SupplierName { get; set; } =string.Empty;

        public DateTime DateImport {  get; set; } = DateTime.MinValue;
    }
}
