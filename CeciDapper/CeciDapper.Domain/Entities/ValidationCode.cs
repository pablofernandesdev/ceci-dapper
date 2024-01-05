using System;

namespace CeciDapper.Domain.Entities
{
    /// <summary>
    /// Represents a validation code entity used for user validation.
    /// </summary>
    public class ValidationCode : BaseEntity
    {
        /// <summary>
        /// Gets or sets the validation code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the validation code.
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Gets a value indicating whether the validation code has expired.
        /// </summary>
        public bool IsExpired => DateTime.UtcNow >= Expires;

        /// <summary>
        /// Gets a value indicating whether the validation code is currently active.
        /// </summary>
        public bool IsActive => !IsExpired;

        /// <summary>
        /// Gets or sets the identifier of the user associated with the validation code.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the validation code.
        /// </summary>
        public virtual User User { get; set; }
    }
}
