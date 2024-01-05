using System.Text.Json.Serialization;

namespace CeciDapper.Domain.DTO.Auth
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the result of an authentication operation.
    /// </summary>
    public class AuthResultDTO
    {
        /// <summary>
        /// Gets or sets the user identifier code.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the JWT (JSON Web Token) issued for the authentication.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the refresh token for the JWT authentication, stored in an HTTP-only cookie.
        /// </summary>
        [JsonIgnore] // The refresh token is returned in an HTTP-only cookie.
        public string RefreshToken { get; set; }
    }
}