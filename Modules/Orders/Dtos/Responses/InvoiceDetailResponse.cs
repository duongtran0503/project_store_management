namespace StoreManagement.API.Modules.Orders.Dtos.Responses
{
    public class InvoiceDetailResponse
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string OrderType { get; set; }
        public decimal Subtotal { get; set; }      
        public decimal TotalDiscount { get; set; } 
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }  
        public DateTime? PaymentTime { get; set; }
        public decimal AmountPaid { get; set; }     
        public decimal ChangeDue { get; set; }     
        public string Status { get; set; }         
        public string? PaymentNote { get; set; }
        public CustomerDTO? Customer { get; set; }
        public StaffDTO? Staff { get; set; }
        public   List<InvoiceDetailDTO> Details { get; set; } = new List<InvoiceDetailDTO>();


    }
}
