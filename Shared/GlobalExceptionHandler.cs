using Microsoft.AspNetCore.Diagnostics;

namespace AbpTask.Shared
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IProblemDetailsService _problemDetailsService;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger,
            IProblemDetailsService problemDetailsService)
        {
            _logger = logger;
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            if (exception is AppException appException)
            {
                httpContext.Response.StatusCode = appException.ExceptionInfo.Type switch
                {
                    AppException.Info.TypeEnum.NotFound => StatusCodes.Status404NotFound,
                    AppException.Info.TypeEnum.Conflict => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };

                return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = {
                        Title = appException.ExceptionInfo.Message,
                        Detail = appException.InnerException?.Message,
                        Status = httpContext.Response.StatusCode,
                        Type = appException.ExceptionInfo.Type.ToString()
                    }
                });
            }



            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails =
            {
                Title = "An error occurred while processing your request.",
                Detail = exception.Message,
                Status = httpContext.Response.StatusCode,
                Type = exception.GetType().Name
            }
            });
        }
    }
}
