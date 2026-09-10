using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Outputs;
using AbpTask.Shared.Interfaces;

namespace AbpTask.Modules.Room.UseCases.Interfaces
{
    public interface ISearchRoomsUseCase : IUseCase<SearchRoomsUseCaseInput, List<SearchRoomsUseCaseOutput>>
    {
    }
}
