namespace AbpTask.Modules.Reservation.UseCases.Outputs
{
    public class CreateReservationUseCaseOutput
    {
        public required long Id { get; set; }
        public required DateTime StartAt { get; set; }
        public required DateTime EndAt { get; set; }
        public required double TotalPrice { get; set; }
        public required List<Service> Services { get; set; }
        public required Room RoomDetails { get; set; }

        public class Room
        {
            public required long Id { get; set; }
            public required string Name { get; set; }
            public required int Capacity { get; set; }
            public required double BasePricePerHour { get; set; }
            public required int PricePercentage { get; set; }
        }

        public class Service
        {
            public required long Id { get; set; }
            public required string Name { get; set; }
            public required double Price { get; set; }
        }
    }
}
