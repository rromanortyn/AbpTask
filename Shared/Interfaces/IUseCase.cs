namespace AbpTask.Shared.Interfaces
{
    public interface IUseCase<InputType, OutputType>
    {
        public OutputType Execute(InputType input);
    }
}
