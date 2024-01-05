using CeciDapper.Infra.CrossCutting.QueryAttributes;
using System.Collections.Generic;

namespace CeciDapper.Domain.Entities
{
    /// <summary>
    /// Represents a user entity in the system.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the role associated with the user.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's email has been validated.
        /// </summary>
        public bool Validated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user needs to change their password.
        /// </summary>
        public bool ChangePassword { get; set; }

        /// <summary>
        /// Gets or sets the role associated with the user.
        /// </summary>
        [IgnoreQuery]
        public virtual Role Role { get; set; }

        /// <summary>
        /// Gets or sets the collection of refresh tokens associated with the user.
        /// </summary>
        [IgnoreQuery]
        public ICollection<RefreshToken> RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the collection of registration tokens associated with the user.
        /// </summary>
        [IgnoreQuery]
        public ICollection<RegistrationToken> RegistrationToken { get; set; }

        /// <summary>
        /// Gets or sets the collection of validation codes associated with the user.
        /// </summary>
        [IgnoreQuery]
        public ICollection<ValidationCode> ValidationCode { get; set; }

        /// <summary>
        /// Gets or sets the collection of addresses associated with the user.
        /// </summary>
        [IgnoreQuery]
        public ICollection<Address> Address { get; set; }
    }
}
