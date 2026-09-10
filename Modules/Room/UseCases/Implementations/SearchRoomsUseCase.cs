using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Interfaces;
using AbpTask.Modules.Room.UseCases.Outputs;

namespace AbpTask.Modules.Room.UseCases.Implementations
{
    public class SearchRoomsUseCase : ISearchRoomsUseCase
    {
        public Task<List<SearchRoomsUseCaseOutput>> Execute(SearchRoomsUseCaseInput input)
        {
            return Task.FromResult(new List<SearchRoomsUseCaseOutput>());
        }
    }
}
