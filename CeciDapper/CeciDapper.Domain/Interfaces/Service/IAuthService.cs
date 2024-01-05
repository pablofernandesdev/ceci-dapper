using CeciDapper.Domain.DTO.Auth;
using CeciDapper.Domain.DTO.Commons;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for authentication and token management.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user asynchronously using their login credentials.
        /// </summary>
        /// <param name="model">The DTO containing the user's login credentials.</param>
        /// <param name="ipAddress">The IP address of the user.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse containing the authentication result.</returns>
        Task<ResultResponse<AuthResultDTO>> AuthenticateAsync(LoginDTO model, string ipAddress);

        /// <summary>
        /// Generates a new access token asynchronously based on a refresh token.
        /// </summary>
        /// <param name="token">The refresh token used to generate a new access token.</param>
        /// <param name="ipAddress">The IP address of the user.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse containing the new authentication result.</returns>
        Task<ResultResponse<AuthResultDTO>> RefreshTokenAsync(string token, string ipAddress);

        /// <summary>
        /// Revokes a user's token asynchronously, making it invalid for future use.
        /// </summary>
        /// <param name="token">The token to revoke.</param>
        /// <param name="ipAddress">The IP address of the user.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the operation.</returns>
        Task<ResultResponse> RevokeTokenAsync(string token, string ipAddress);

        /// <summary>
        /// Initiates the process for recovering a forgotten password.
        /// </summary>
        /// <param name="model">The DTO containing the user's email address for password recovery.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the operation.</returns>
        Task<ResultResponse> ForgotPasswordAsync(ForgotPasswordDTO model);
    }
}
