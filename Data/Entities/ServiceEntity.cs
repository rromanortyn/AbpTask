using System.ComponentModel.DataAnnotations;

namespace AbpTask.Data.Entities
{
    public class ServiceEntity : IBaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, double.MaxValue)]
        public double Price { get; set; }

        public long RoomId { get; set; }
        public RoomEntity Room { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
