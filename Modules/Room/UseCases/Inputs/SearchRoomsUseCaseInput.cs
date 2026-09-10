namespace AbpTask.Modules.Room.UseCases.Inputs
{
    public class SearchRoomsUseCaseInput
    {
        public required DateTime StartAt { get; set; }
        public required DateTime EndAt { get; set; }
        public required int Capacity { get; set; }
    }
}
