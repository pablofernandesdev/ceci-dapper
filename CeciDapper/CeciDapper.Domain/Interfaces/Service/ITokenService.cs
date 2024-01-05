using CeciDapper.Domain.DTO.User;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for generating authentication tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates an authentication token based on the provided user information.
        /// </summary>
        /// <param name="model">The DTO containing the user information.</param>
        /// <returns>A string representing the generated authentication token.</returns>
        string GenerateToken(UserResultDTO model);
    }
}
