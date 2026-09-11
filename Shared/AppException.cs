namespace AbpTask.Shared
{
    public class AppException : Exception
    {
        public Info ExceptionInfo { get; set; }

        public AppException(Info info, Exception innerException) : base(info.Message, innerException)
        {
            ExceptionInfo = info;
        }

        public class Info
        {
            public required TypeEnum Type { get; set; }
            public required string Code { get; set; }
            public required string Message { get; set; }

            public enum TypeEnum
            {
                NotFound,
                Conflict,
            }
        }
    }
}
