using System.ComponentModel.DataAnnotations;

namespace CeciDapper.Domain.DTO.Register
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the user self-registration information.
    /// </summary>
    public class UserSelfRegistrationDTO
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}