using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Common.Entities
{
    public class VoucherTarget :BaseEntity
    {
         public string TargetId { get; set; }
        public string VoucherId { get; set; }

        public string TargetType { get; set; }
        public Voucher Voucher { get; set; }
    }
}
