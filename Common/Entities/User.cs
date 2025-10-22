namespace StoreManagement.API.Shared.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = "staff";
        public bool IsActive { get; set; } = true;
        public DateTime? LastLogin { get; set; }

        // Navigation Properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        public virtual ICollection<InventoryHistory> InventoryHistories { get; set; } = new List<InventoryHistory>();
    }
}
