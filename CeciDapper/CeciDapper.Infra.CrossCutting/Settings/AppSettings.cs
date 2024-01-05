using System.Diagnostics.CodeAnalysis;

namespace CeciDapper.Infra.CrossCutting.Settings
{
    /// <summary>
    /// Represents the application settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the connection strings.
        /// </summary>
        public ConnectionStrings ConnectionStrings { get; set; }

        /// <summary>
        /// Gets or sets the Swagger settings.
        /// </summary>
        public SwaggerSettings SwaggerSettings { get; set; }

        /// <summary>
        /// Gets or sets the external providers.
        /// </summary>
        public ExternalProviders ExternalProviders { get; set; }

        /// <summary>
        /// Gets or sets the SendGrid settings.
        /// </summary>
        public SendGrid SendGrid { get; set; }

        /// <summary>
        /// Gets or sets the Firebase settings.
        /// </summary>
        public Firebase Firebase { get; set; }

        /// <summary>
        /// Gets or sets the ViaCep settings.
        /// </summary>
        public ViaCep ViaCep { get; set; }

        /// <summary>
        /// Gets or sets the email settings.
        /// </summary>
        public EmailSettings EmailSettings { get; set; }

        /// <summary>
        /// Gets or sets the role settings.
        /// </summary>
        public RoleSettings RoleSettings { get; set; }
    }

    /// <summary>
    /// Represents the connection strings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConnectionStrings
    {
        /// <summary>
        /// Gets or sets the CeciDatabase connection string.
        /// </summary>
        public string CeciDatabase { get; set; }
    }

    /// <summary>
    /// Represents the Swagger settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerSettings
    {
        /// <summary>
        /// Gets or sets the authorized Swagger user.
        /// </summary>
        public string SwaggerUserAuthorized { get; set; }

        /// <summary>
        /// Gets or sets the Swagger authorized user password.
        /// </summary>
        public string SwaggerAuthorizedPassword { get; set; }
    }

    /// <summary>
    /// Represents the external providers.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExternalProviders
    {
        /// <summary>
        /// Gets or sets the SendGrid settings.
        /// </summary>
        public SendGrid SendGrid { get; set; }

        /// <summary>
        /// Gets or sets the Firebase settings.
        /// </summary>
        public Firebase Firebase { get; set; }

        /// <summary>
        /// Gets or sets the ViaCep settings.
        /// </summary>
        public ViaCep ViaCep { get; set; }
    }

    /// <summary>
    /// Represents the SendGrid settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SendGrid
    {
        /// <summary>
        /// Gets or sets the SendGrid API key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the sender email.
        /// </summary>
        public string SenderEmail { get; set; }

        /// <summary>
        /// Gets or sets the sender name.
        /// </summary>
        public string SenderName { get; set; }
    }

    /// <summary>
    /// Represents the Firebase settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Firebase
    {
        /// <summary>
        /// Gets or sets the Firebase server API key.
        /// </summary>
        public string ServerApiKey { get; set; }

        /// <summary>
        /// Gets or sets the Firebase sender ID.
        /// </summary>
        public string SenderId { get; set; }
    }

    /// <summary>
    /// Represents the ViaCep settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ViaCep
    {
        /// <summary>
        /// Gets or sets the ViaCep API URL.
        /// </summary>
        public string ApiUrl { get; set; }
    }

    /// <summary>
    /// Represents the email settings.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the email host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the email port.
        /// </summary>
        public int Port { get; set; }
    }

    /// <summary>
    /// Represents the role settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RoleSettings
    {
        /// <summary>
        /// Gets or sets the basic role name.
        /// </summary>
        public string BasicRoleName { get; set; }
    }
}
