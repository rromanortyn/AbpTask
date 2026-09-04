using System.ComponentModel.DataAnnotations;

namespace AbpTask.Modules.Room.Dtos.Request
{
    public class CreateRoomRequestDto
    {
        [Required]
        [MinLength(1)]
        public required string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public required int Capacity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public required double BasePricePerHour { get; set; }

        [Required]

        [Length(1, int.MaxValue, ErrorMessage = "At least one service is required.")]
        public ICollection<Service> Services { get; set; } = [];

        public class Service
        {
            [Required]
            [MinLength(1)]
            public string Name { get; set; } = string.Empty;

            [Required]
            [Range(1, double.MaxValue)]
            public double Price { get; set; }
        }
    }
}
