using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Shared.Interfaces;

namespace AbpTask.Modules.Room.UseCases.Interfaces
{
    public interface IDeleteRoomUseCase : IUseCase<DeleteRoomUseCaseInput, object>
    {
    }
}
