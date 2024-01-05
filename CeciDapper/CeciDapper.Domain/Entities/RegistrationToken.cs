namespace CeciDapper.Domain.Entities
{
    /// <summary>
    /// Represents a registration token entity used for user registration confirmation.
    /// </summary>
    public class RegistrationToken : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the user associated with the registration token.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the registration token value.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the user associated with the registration token.
        /// </summary>
        public virtual User User { get; set; }
    }
}
