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
    }
}
