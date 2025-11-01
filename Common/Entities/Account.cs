using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class Account : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string RoleName { get; set; } = "Staff"; 
        public bool IsActive { get; set; } = true;
        public DateTime? HireDate { get; set; }

  
        public virtual ICollection<ShiftHistory> Shifts { get; set; } = new List<ShiftHistory>();
        public virtual ICollection<InventoryReceipt> ReceivedReceipts { get; set; } = new List<InventoryReceipt>();
        public virtual ICollection<Invoice> CashierInvoices { get; set; } = new List<Invoice>();
    }
}
