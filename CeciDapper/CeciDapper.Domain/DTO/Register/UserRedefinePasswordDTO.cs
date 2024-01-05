namespace CeciDapper.Domain.DTO.Register
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the user's current and new password for redefining the password.
    /// </summary>
    public class UserRedefinePasswordDTO
    {
        /// <summary>
        /// Gets or sets the current password of the user.
        /// </summary>
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password for the user.
        /// </summary>
        public string NewPassword { get; set; }
    }
}