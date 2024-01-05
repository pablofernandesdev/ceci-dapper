namespace CeciDapper.Domain.DTO.Auth
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the user credentials for the login operation.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}