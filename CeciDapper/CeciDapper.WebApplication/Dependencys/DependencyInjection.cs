using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using CeciDapper.Domain.Interfaces.Service.External;
using CeciDapper.Infra.Data.Repository;
using CeciDapper.Service.Services;
using CeciDapper.Service.Services.External;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace CeciDapper.WebApplication.Dependencys
{
    /// <summary>
    /// Static class that contains extension methods for dependency injection configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds repositories as services to the dependency injection container.
        /// </summary>
        /// <param name="repositorys">The collection of repository services.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The updated collection of repository services.</returns>
        public static IServiceCollection AddRepository(this IServiceCollection repositorys, IConfiguration configuration)
        {
            //repositorys
            repositorys.AddTransient<IRoleRepository, RoleRepository>();
            repositorys.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            repositorys.AddTransient<IUserRepository, UserRepository>();
            repositorys.AddTransient<IRegistrationTokenRepository, RegistrationTokenRepository>();
            repositorys.AddTransient<IValidationCodeRepository, ValidationCodeRepository>();
            repositorys.AddTransient<IAddressRepository, AddressRepository>();
            repositorys.AddTransient<IUnitOfWork, UnitOfWork>();

            return repositorys;
        }

        /// <summary>
        /// Adds services as services to the dependency injection container.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The updated collection of services.</returns>
        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IImportService, ImportService>();
            services.AddTransient<IValidationCodeService, ValidationCodeService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IRegisterService, RegisterService>();

            // External services
            services.AddTransient<ISendGridService, SendGridService>();
            //services.AddTransient<IFirebaseService, FirebaseService>();

            // HttpClient configuration for the Firebase service
            services.AddHttpClient<IFirebaseService, FirebaseService>(client =>
            {
                var firebaseOptionsServerId = configuration["ExternalProviders:Firebase:ServerApiKey"];
                var firebaseOptionsSenderId = configuration["ExternalProviders:Firebase:SenderId"];

                client.BaseAddress = new Uri("https://fcm.googleapis.com");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                    $"key={firebaseOptionsServerId}");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={firebaseOptionsSenderId}");
            });

            // HttpClient configuration for the ViaCep service
            services.AddHttpClient<IViaCepService, ViaCepService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ExternalProviders:ViaCep:ApiUrl"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            return services;
        }
    }
}
