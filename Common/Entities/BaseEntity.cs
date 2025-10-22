using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Shared.Entities
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
