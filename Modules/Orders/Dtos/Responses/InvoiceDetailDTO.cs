namespace StoreManagement.API.Modules.Orders.Dtos.Responses
{
    public class InvoiceDetailDTO

    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal ItemSubtotal { get; set; }
        public decimal FinalItemAmount { get; set; } 

      
        public string BookId { get; set; }

        public string BookImage { get; set; }
        public string BookTitle { get; set; } 

       
       public VoucherDTO? Voucher { get; set; }
    }
}
