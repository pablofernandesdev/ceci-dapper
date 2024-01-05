using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CeciDapper.Domain.DTO.Email
{
    /// <summary>
    /// Data Transfer Object (DTO) representing an email request to be sent.
    /// </summary>
    public class EmailRequestDTO
    {
        /// <summary>
        /// Gets or sets the recipient email address.
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body of the email.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the list of file attachments to be included in the email.
        /// </summary>
        public List<IFormFile> Attachments { get; set; }
    }
}