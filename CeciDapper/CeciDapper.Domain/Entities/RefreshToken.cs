using System;

namespace CeciDapper.Domain.Entities
{
    /// <summary>
    /// Represents a refresh token entity used for user authentication token management.
    /// </summary>
    public class RefreshToken : BaseEntity
    {
        /// <summary>
        /// Gets or sets the refresh token value.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the refresh token.
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Gets a value indicating whether the refresh token is expired.
        /// </summary>
        public bool IsExpired => DateTime.UtcNow >= Expires;

        /// <summary>
        /// Gets or sets the IP address from which the refresh token was created.
        /// </summary>
        public string CreatedByIp { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the refresh token was revoked.
        /// </summary>
        public DateTime? Revoked { get; set; }

        /// <summary>
        /// Gets or sets the IP address from which the refresh token was revoked.
        /// </summary>
        public string RevokedByIp { get; set; }

        /// <summary>
        /// Gets or sets the replacement token for this refresh token.
        /// </summary>
        public string ReplacedByToken { get; set; }

        /// <summary>
        /// Gets a value indicating whether the refresh token is currently active.
        /// </summary>
        public bool IsActive => Revoked == null && !IsExpired;

        /// <summary>
        /// Gets or sets the identifier of the user associated with the refresh token.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the user associated with the refresh token.
        /// </summary>
        public virtual User User { get; set; }
    }
}
