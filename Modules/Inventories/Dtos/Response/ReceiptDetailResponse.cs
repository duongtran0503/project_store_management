namespace StoreManagement.API.Modules.Inventories.Dtos.Response
{
    public class ReceiptDetailResponse
    {
        public string BookId { get; set; }
        public string BookTitle { get; set; } 
        public int QuantityReceived { get; set; } 

        public string? BookName { get; set; }
        public string? BookImage {  get; set; }
        public decimal UnitCost { get; set; }


        public decimal TotalLineCost { get; set; }
    }
}
