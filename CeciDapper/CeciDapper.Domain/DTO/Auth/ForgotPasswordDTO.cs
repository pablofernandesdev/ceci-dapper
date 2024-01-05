namespace CeciDapper.Domain.DTO.Auth
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the user email for the "forgot password" operation.
    /// </summary>
    public class ForgotPasswordDTO
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }
    }
}