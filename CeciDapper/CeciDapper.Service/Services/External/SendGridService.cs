using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.Interfaces.Service.External;
using CeciDapper.Infra.CrossCutting.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services.External
{
    /// <summary>
    /// Service responsible for sending emails using SendGrid.
    /// </summary>
    public class SendGridService : ISendGridService
    {
        private readonly ExternalProviders _externalProviders;

        /// <summary>
        /// Initializes a new instance of the SendGridService class.
        /// </summary>
        /// <param name="externalProviders">Configuration options for external providers.</param>
        public SendGridService(IOptions<ExternalProviders> externalProviders)
        {
            _externalProviders = externalProviders.Value;
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="email">Recipient email address.</param>
        /// <param name="subject">Email subject.</param>
        /// <param name="message">Email message content.</param>
        /// <returns>ResultResponse object containing the operation status.</returns>
        public async Task<ResultResponse> SendEmailAsync(string email, string subject, string message)
        {
            var response = new ResultResponse();

            try
            {
                var result = await Execute(_externalProviders.SendGrid.ApiKey, subject, message, email);

                response.StatusCode = result.StatusCode;
                response.Message = await result.Body.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Log the exception here
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = $"An error occurred while sending the email: {ex.Message}";
            }

            return response;
        }

        private async Task<Response> Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_externalProviders.SendGrid.SenderEmail, _externalProviders.SendGrid.SenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable tracking settings
            // ref.: https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            msg.SetOpenTracking(false);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(false);

            return await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}