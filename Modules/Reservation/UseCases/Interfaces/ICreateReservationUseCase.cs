using AbpTask.Modules.Reservation.UseCases.Inputs;
using AbpTask.Modules.Reservation.UseCases.Outputs;
using AbpTask.Shared.Interfaces;

namespace AbpTask.Modules.Reservation.UseCases.Interfaces
{
    public interface ICreateReservationUseCase : IUseCase<CreateReservationUseCaseInput, CreateReservationUseCaseOutput>
    {
    }
}
