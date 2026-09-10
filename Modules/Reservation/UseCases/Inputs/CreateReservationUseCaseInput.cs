using System.ComponentModel.DataAnnotations;

namespace AbpTask.Modules.Reservation.UseCases.Inputs
{
    public class CreateReservationUseCaseInput
    {
        public long RoomId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public List<long> ChosenServicesIds { get; set; } = [];
    }
}
