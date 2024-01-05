using CeciDapper.WebApplication.Middlewares;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics.CodeAnalysis;

namespace CeciDapper.WebApplication.Extensions
{
    /// <summary>
    /// Extension class that provides a method to add basic authorization to Swagger.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SwaggerAuthorizeExtensions
    {
        /// <summary>
        /// Adds the basic authorization middleware to Swagger in the ASP.NET Core pipeline.
        /// </summary>
        /// <param name="builder">The IApplicationBuilder object.</param>
        /// <returns>The IApplicationBuilder object to allow chained calls.</returns>
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerBasicAuthMiddleware>();
        }
    }
}
