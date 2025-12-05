namespace StoreManagement.API.Modules.Orders.Dtos.Responses
{
    public class SummaryInvoiceDetailDTO
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
    }
}
