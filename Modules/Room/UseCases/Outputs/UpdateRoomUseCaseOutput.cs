namespace AbpTask.Modules.Room.UseCases.Outputs
{
    public class UpdateRoomUseCaseOutput
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
        public required int Capacity { get; set; }
        public required double BasePricePerHour { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public required ICollection<Service> Services { get; set; } = [];

        public class Service
        {
            public required long Id { get; set; }
            public required string Name { get; set; } = string.Empty;
            public required double Price { get; set; }
            public required DateTime CreatedAt { get; set; }
            public required DateTime UpdatedAt { get; set; }
        }
    }
}
