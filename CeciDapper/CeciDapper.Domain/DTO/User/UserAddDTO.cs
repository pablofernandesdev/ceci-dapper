using System.ComponentModel.DataAnnotations;

namespace CeciDapper.Domain.DTO.User
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the addition of a new user.
    /// </summary>
    public class UserAddDTO
    {
        /// <summary>
        /// Gets or sets the name of the new user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the new user.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the new user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the RoleId of the role assigned to the new user.
        /// </summary>
        public int RoleId { get; set; }
    }
}
