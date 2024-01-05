using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Email;
using CeciDapper.Domain.Interfaces.Service;
using CeciDapper.Infra.CrossCutting.Settings;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service responsible for sending emails.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="emailSettings">The email settings.</param>
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="emailRequest">The email request.</param>
        /// <returns>A response indicating the success of the email sending.</returns>
        public async Task<ResultResponse> SendEmailAsync(EmailRequestDTO emailRequest)
        {
            var response = new ResultResponse();

            try
            {
                using (var message = BuildMailMessage(emailRequest))
                using (var smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port))
                {
                    smtp.Port = _emailSettings.Port;
                    smtp.Host = _emailSettings.Host;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_emailSettings.Mail, _emailSettings.Password);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = "Failed to send the email.";
                response.Exception = ex;
            }

            return response;
        }

        private MailMessage BuildMailMessage(EmailRequestDTO emailRequest)
        {
            var message = new MailMessage();

            message.From = new MailAddress(_emailSettings.Mail, _emailSettings.DisplayName);
            message.To.Add(new MailAddress(emailRequest.ToEmail));
            message.Subject = emailRequest.Subject;
            message.IsBodyHtml = false;
            message.Body = emailRequest.Body;

            if (emailRequest.Attachments != null)
            {
                foreach (var file in emailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            file.CopyTo(memoryStream);
                            var fileBytes = memoryStream.ToArray();
                            Attachment attachment = new Attachment(new MemoryStream(fileBytes), file.FileName);
                            message.Attachments.Add(attachment);
                        }
                    }
                }
            }

            return message;
        }
    }
}