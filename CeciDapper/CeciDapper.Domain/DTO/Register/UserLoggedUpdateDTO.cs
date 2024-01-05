using System.ComponentModel.DataAnnotations;

namespace CeciDapper.Domain.DTO.Register
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the user information to be updated for a logged-in user.
    /// </summary>
    public class UserLoggedUpdateDTO
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }
    }
}