using CeciDapper.Domain.DTO.Commons;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service.External
{
    /// <summary>
    /// Represents a service interface for sending emails using the SendGrid service.
    /// </summary>
    public interface ISendGridService
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The content of the email message.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the success or failure of the email sending.</returns>
        Task<ResultResponse> SendEmailAsync(string email, string subject, string message);
    }
}
