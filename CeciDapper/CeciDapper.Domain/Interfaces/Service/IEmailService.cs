using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Email;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously using the provided email request.
        /// </summary>
        /// <param name="emailRequest">The DTO containing email content and recipient information.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the email sending operation.</returns>
        Task<ResultResponse> SendEmailAsync(EmailRequestDTO emailRequest);
    }
}
