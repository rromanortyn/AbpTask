using System.ComponentModel.DataAnnotations;

namespace AbpTask.Data.Entities
{
    public class ChosenServiceEntity : IBaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long ServiceId { get; set; }

        [Required]
        public ServiceEntity Service { get; set; } = null!;

        [Required]
        public long ReservationId { get; set; }

        [Required]
        public ReservationEntity Reservation { get; set; } = null!;

        [Required]
        public double Price { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
