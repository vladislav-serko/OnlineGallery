using Microsoft.AspNetCore.Builder;
using OnlineGallery.Api.Middleware;

namespace OnlineGallery.Api.Extensions
{
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}