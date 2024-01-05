using CeciDapper.Infra.CrossCutting.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CeciDapper.WebApplication.Middlewares
{
    /// <summary>
    /// Middleware class provides basic authentication for the Swagger documentation endpoint in the ASP.NET Core application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SwaggerSettings _swaggerSettings;

        /// <summary>
        /// Initializes a new instance of the SwaggerBasicAuthMiddleware class with the specified dependencies.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="swaggerSettings">The Swagger settings provided through dependency injection.</param>
        public SwaggerBasicAuthMiddleware(RequestDelegate next,
            IOptions<SwaggerSettings> swaggerSettings)
        {
            _next = next;
            _swaggerSettings = swaggerSettings.Value;
        }

        /// <summary>
        /// Invokes the middleware to provide basic authentication for the Swagger documentation endpoint.
        /// </summary>
        /// <param name="context">The HttpContext representing the current HTTP request.</param>
        /// <returns>A Task representing the asynchronous completion of the middleware operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            //Make sure we are hitting the swagger path, and not doing it locally as it just gets annoying :-)
            if (context.Request.Path.StartsWithSegments("/swagger") /*&& !this.IsLocalRequest(context)*/)
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the encoded username and password
                    var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                    // Decode from Base64 to string
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    // Split username and password
                    var username = decodedUsernamePassword.Split(':', 2)[0];
                    var password = decodedUsernamePassword.Split(':', 2)[1];

                    // Check if login is correct
                    if (IsAuthorized(username, password))
                    {
                        await _next.Invoke(context);
                        return;
                    }
                }

                // Return authentication type (causes browser to show login dialog)
                context.Response.Headers["WWW-Authenticate"] = "Basic";

                // Return unauthorized
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        /// <summary>
        /// Checks if the provided username and password are authorized for accessing the Hangfire Dashboard.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="password">The password to check.</param>
        /// <returns>true if the username and password are authorized; otherwise, false.</returns>
        public bool IsAuthorized(string username, string password)
        {
            // Check that username and password are correct
            return username.Equals(_swaggerSettings.SwaggerUserAuthorized, StringComparison.InvariantCultureIgnoreCase)
                    && password.Equals(_swaggerSettings.SwaggerAuthorizedPassword);
        }

        /// <summary>
        /// Checks if the current HTTP request is a local request.
        /// </summary>
        /// <param name="context">The HttpContext representing the current HTTP request.</param>
        /// <returns>true if the request is local; otherwise, false.</returns>
        public bool IsLocalRequest(HttpContext context)
        {
            //Handle running using the Microsoft.AspNetCore.TestHost and the site being run entirely locally in memory without an actual TCP/IP connection
            if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
            {
                return true;
            }
            if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
            {
                return true;
            }
            if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
            {
                return true;
            }
            return false;
        }
    }
}
