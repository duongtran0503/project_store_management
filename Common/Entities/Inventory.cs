using StoreManagement.API.Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.API.Common.Entities
{
    public class Inventory:BaseEntity
    {
        public string BookId { get; set; } = string.Empty;

        public int AvailableStock { get; set; } = 0;

        public int ReservedStock { get; set; } = 0;
       
        [NotMapped] 
        public int StockCanBeSold => AvailableStock - ReservedStock;

        public virtual Book Book { get; set; } = default!;
    }
}
