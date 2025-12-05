namespace StoreManagement.API.Modules.Inventories.Dtos.Response
{
    public class InventoryResponse
    {
        public string BookId { get; set; }
        public string BookName { get; set; }
        public string SKU { get; set; }

        public int AvailableStock { get; set; } 
        public int ReservedStock { get; set; }  
        public int MinStockLevel { get; set; }

        public decimal AverageCostPrice { get; set; }

        public decimal TotalInventoryValue { get; set; }


    }
}
