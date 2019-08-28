using Microsoft.AspNetCore.Builder;
using Stores.API.Infrastructure.Middlewares;

namespace Stores.API.Infrastructure.Extensions
{
    public static class ErrorLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}
