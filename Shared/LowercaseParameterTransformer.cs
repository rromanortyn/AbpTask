using Microsoft.AspNetCore.Routing;

namespace AbpTask.Shared
{
    public class LowercaseParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            return value?.ToString()?.ToLowerInvariant();
        }
    }
}
