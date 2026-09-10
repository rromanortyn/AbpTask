using System.ComponentModel.DataAnnotations;

namespace AbpTask.Data.Entities
{
    public class ReservationEntity : IBaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long RoomId { get; set; }

        [Required]
        public RoomEntity Room { get; set; } = null!;

        [Required]
        public List<ChosenServiceEntity> ChosenServices { get; set; } = [];

        [Required]
        public DateTime StartAt { get; set; }
        [Required]
        public DateTime EndAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
