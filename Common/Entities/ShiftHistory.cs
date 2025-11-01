using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class ShiftHistory : BaseEntity
    {
        public string StaffId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? CashStart { get; set; }
        public decimal? CashEnd { get; set; }
        public virtual Account Staff { get; set; } = default!;
    }
}
