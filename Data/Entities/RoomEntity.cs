using System.ComponentModel.DataAnnotations;

namespace AbpTask.Data.Entities
{
    public class RoomEntity : IBaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Capacity { get; set; }

        [Required]
        public double BasePricePerHour { get; set; }

        public ICollection<ServiceEntity> Services { get; set; } = [];

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
