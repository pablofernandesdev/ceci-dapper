using System.Collections.Generic;

namespace CeciDapper.Domain.Entities
{
    /// <summary>
    /// Represents a role entity for user authorization and access control.
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of users associated with this role.
        /// </summary>
        public ICollection<User> User { get; set; }
    }
}
