namespace StoreManagement.API.Modules.Inventories.Dtos.Response
{
    public class InventoryReceiptResponse
    {
        public string Id { get; set; }
        public DateTime ReceiptDate { get; set; } 
        public decimal TotalCost { get; set; } 
        public string ReceivingStaffId { get; set; }

        public string ReceivingStaffName { get; set; }  
        public string SupplierId { get; set; }

        public string SupplierName { get; set; }    

        public string GRNStatus { get; set; }

        public List<ReceiptDetailResponse> Details { get; set; } = new List<ReceiptDetailResponse>();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
