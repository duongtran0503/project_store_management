using StoreManagement.API.Modules.Products.Constants;
using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string PublisherId { get; set; } = string.Empty;
        public string Image { get; set; } = "https://firebasestorage.googleapis.com/v0/b/todo-app-1fe54.appspot.com/o/books-image%2Fno-image.jpg?alt=media&token=de5eea16-4de9-49eb-ba00-fa8946e41276";
        public string Isbn { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public string AuthorId {  get; set; } = string.Empty;
        public decimal RetailPrice { get; set; }

        public string Status { get; set; } = ProductConstants.PRODUCT_STATUS_SELL_ACTIVE;
       
        public virtual Category Category { get; set; } = default!;
        public virtual Publisher Publisher { get; set; } = default!;
        public virtual Author Author { get; set; } = default!;

        public virtual Inventory? Inventory { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();
    }
}
