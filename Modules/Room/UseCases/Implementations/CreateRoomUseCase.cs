using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Interfaces;
using AbpTask.Modules.Room.UseCases.Outputs;

namespace AbpTask.Modules.Room.UseCases.Implementations
{
    public class CreateRoomUseCase : ICreateRoomUseCase
    {
        public CreateRoomUseCaseOutput Execute(CreateRoomUseCaseInput input)
        {
            return new CreateRoomUseCaseOutput { Id = 1 };
        }
    }
}
