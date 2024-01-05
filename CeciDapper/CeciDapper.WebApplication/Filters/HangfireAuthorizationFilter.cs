using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Net;
using System.Text;

namespace CeciDapper.WebApplication.Filters
{
    /// <summary>
    /// Filter class that provides basic authorization for Hangfire Dashboard.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _user;
        private readonly string _password;

        /// <summary>
        /// Initializes a new instance of the HangfireAuthorizationFilter class with the specified user and password.
        /// </summary>
        /// <param name="user">The username for basic authentication.</param>
        /// <param name="password">The password for basic authentication.</param>
        public HangfireAuthorizationFilter(string user, string password)
        {
            _user = user;
            _password = password;
        }

        /// <summary>
        /// Authorizes requests to the Hangfire Dashboard using basic authentication.
        /// </summary>
        /// <param name="context">The DashboardContext containing the current HttpContext.</param>
        /// <returns>true if the user is authorized; otherwise, false.</returns>
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            string authHeader = httpContext.Request.Headers["Authorization"];

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
                    return true;
                }
            }

            // Return authentication type (causes browser to show login dialog)
            httpContext.Response.Headers["WWW-Authenticate"] = "Basic";

            // Return unauthorized
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            return false;
        }

        /// <summary>
        /// Checks if the provided username and password are authorized for accessing the Hangfire Dashboard.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="password">The password to check.</param>
        /// <returns>true if the username and password are authorized; otherwise, false.</returns>
        private bool IsAuthorized(string username, string password)
        {
            // Check that username and password are correct
            return username.Equals(_user, StringComparison.InvariantCultureIgnoreCase)
                    && password.Equals(_password);
        }
    }
}
