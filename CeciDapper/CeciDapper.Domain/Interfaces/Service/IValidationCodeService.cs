using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.ValidationCode;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for managing validation code-related operations.
    /// </summary>
    public interface IValidationCodeService
    {
        /// <summary>
        /// Sends a validation code to the user.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with the result of the code sending.</returns>
        Task<ResultResponse> SendAsync();

        /// <summary>
        /// Validates a provided validation code.
        /// </summary>
        /// <param name="obj">The DTO containing the validation code to be validated.</param>
        /// <returns>A task representing the asynchronous operation with the result of the validation.</returns>
        Task<ResultResponse> ValidateCodeAsync(ValidationCodeValidateDTO obj);
    }
}
