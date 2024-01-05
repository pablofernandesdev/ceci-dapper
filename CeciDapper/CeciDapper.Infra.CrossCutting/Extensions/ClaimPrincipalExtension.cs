using System;
using System.Security.Claims;

namespace CeciDapper.Infra.CrossCutting.Extensions
{
    /// <summary>
    /// Provides extension methods for working with ClaimsPrincipal objects.
    /// </summary>
    public static class ClaimPrincipalExtension
    {
        /// <summary>
        /// Gets the ID of the logged-in user from the ClaimsPrincipal object.
        /// </summary>
        /// <typeparam name="T">The type of the ID to retrieve (string, int, or long).</typeparam>
        /// <param name="principal">The ClaimsPrincipal object.</param>
        /// <returns>The ID of the logged-in user.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the principal is null.</exception>
        /// <exception cref="Exception">Thrown when an invalid type is provided.</exception>
        public static T GetLoggedInUserId<T>(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var loggedInUserId = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(loggedInUserId, typeof(T));
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                return loggedInUserId != null ? (T)Convert.ChangeType(loggedInUserId, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));
            }
            else
            {
                throw new Exception("Invalid type provided");
            }
        }

        /// <summary>
        /// Gets the ID of the logged-in user as a string from the ClaimsPrincipal object.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal object.</param>
        /// <returns>The ID of the logged-in user as a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the principal is null.</exception>
        public static string GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Gets the name of the logged-in user from the ClaimsPrincipal object.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal object.</param>
        /// <returns>The name of the logged-in user as a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the principal is null.</exception>
        public static string GetLoggedInUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.Name)?.Value;
        }

        /// <summary>
        /// Gets the email of the logged-in user from the ClaimsPrincipal object.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal object.</param>
        /// <returns>The email of the logged-in user as a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the principal is null.</exception>
        public static string GetLoggedInUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.Email)?.Value;
        }

        /// <summary>
        /// Gets the value of a specific claim type from the ClaimsPrincipal object.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal object.</param>
        /// <param name="type">The type of the claim to retrieve.</param>
        /// <returns>The value of the specified claim as a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the principal or type is null.</exception>
        public static string GetByType(this ClaimsPrincipal principal, string type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return principal.FindFirst(type)?.Value;
        }
    }
}
