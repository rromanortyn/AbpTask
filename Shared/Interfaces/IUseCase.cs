namespace AbpTask.Shared.Interfaces
{
    public interface IUseCase<InputType, OutputType>
    {
        public Task<OutputType> Execute(InputType input);
    }
}
