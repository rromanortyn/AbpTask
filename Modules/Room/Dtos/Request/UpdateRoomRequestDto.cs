using System.ComponentModel.DataAnnotations;

namespace AbpTask.Modules.Room.Dtos.Request
{
    public class UpdateRoomRequestDto
    {
        [MinLength(1)]
        public string? Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int? Capacity { get; set; }

        [Range(0, double.MaxValue)]
        public double? BasePricePerHour { get; set; }

        public List<Service>? ServicesToAdd { get; set; } = [];
        public List<long>? ServicesToDelete { get; set; } = [];
        public class Service
        {
            [MinLength(1)]
            public string Name { get; set; } = string.Empty;

            [Range(1, double.MaxValue)]
            public double Price { get; set; }
        }
    }
}
