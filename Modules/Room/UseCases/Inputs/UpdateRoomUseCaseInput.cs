namespace AbpTask.Modules.Room.UseCases.Inputs
{
    public class UpdateRoomUseCaseInput
    {
        public long Id { get; set; }

        public string? Name { get; set { field = value; HasName = true; } } = string.Empty;
        public bool HasName { get; private set; }

        public int? Capacity { get; set { field = value; HasCapacity = true; } }
        public bool HasCapacity { get; private set; }

        public double? BasePricePerHour { get; set { field = value; HasBasePricePerHour = true; } }
        public bool HasBasePricePerHour { get; private set; }

        public List<Service> ServicesToAdd { get; set; } = [];
        public List<long> ServicesToDelete { get; set; } = [];


        public class Service
        {
            public string Name { get; set; } = string.Empty;
            public double Price { get; set; }
        }
    }
}
