using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class Customer:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; }  =string.Empty; 
        public string Phone { get; set; } = string.Empty;

        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
