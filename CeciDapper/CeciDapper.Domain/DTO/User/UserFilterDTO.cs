using CeciDapper.Domain.DTO.Commons;
using System.ComponentModel.DataAnnotations;

namespace CeciDapper.Domain.DTO.User
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the filtering criteria for retrieving users.
    /// </summary>
    public class UserFilterDTO : QueryFilter
    {
        /// <summary>
        /// Gets or sets the name of the user to filter by.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user to filter by.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the RoleId of the user to filter by.
        /// </summary>
        public int RoleId { get; set; }
    }
}
