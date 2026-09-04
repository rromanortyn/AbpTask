namespace AbpTask.Modules.Room.UseCases.Inputs
{
    public class CreateRoomUseCaseInput
    {
        public required string Name { get; set; }
        public required int Capacity { get; set; }
        public required double BasePricePerHour { get; set; }
        public required ICollection<Service> Services { get; set; }

        public class Service
        {
            public string Name { get; set; } = string.Empty;
            public double Price { get; set; }
        }
    }
}
