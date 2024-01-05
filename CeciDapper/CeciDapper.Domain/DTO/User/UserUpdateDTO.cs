using System.ComponentModel.DataAnnotations;

namespace CeciDapper.Domain.DTO.User
{
    /// <summary>
    /// Data Transfer Object (DTO) representing user information for updating.
    /// </summary>
    public class UserUpdateDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user to be updated.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the updated name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the updated email address of the user.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the updated password of the user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the updated role identifier associated with the user.
        /// </summary>
        public int RoleId { get; set; }
    }
}
