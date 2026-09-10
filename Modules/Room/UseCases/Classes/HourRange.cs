namespace AbpTask.Modules.Room.UseCases.Classes
{
    public class HourRange
    {
        public required TimeSpan Start { get; set; }
        public required TimeSpan End { get; set; }
        public required HourRangeType Type { get; set; }
        public enum HourRangeType
        {
            Morning,
            Standard,
            Rush,
            Evening,
        }
    }
}
