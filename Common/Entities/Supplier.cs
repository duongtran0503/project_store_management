using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class Supplier : BaseEntity
    {
        public string SupplierName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public virtual ICollection<InventoryReceipt> InventoryReceipts { get; set; } = new List<InventoryReceipt>();
    }
}
