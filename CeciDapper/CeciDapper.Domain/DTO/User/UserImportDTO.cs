using System.ComponentModel.DataAnnotations;

namespace CeciDapper.Domain.DTO.User
{
    /// <summary>
    /// Data Transfer Object (DTO) representing user information for importing.
    /// </summary>
    public class UserImportDTO
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
        /// Gets or sets the password of the user (in plain text).
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Base64 encoded password of the user (to be used for decoding).
        /// </summary>
        public string PasswordBase64Decode { get; set; }

        /// <summary>
        /// Gets or sets the role identifier associated with the user.
        /// </summary>
        public int RoleId { get; set; }
    }
}
