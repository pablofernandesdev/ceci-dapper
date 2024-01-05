using AutoMapper;
using CeciDapper.Domain.DTO.Auth;
using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Email;
using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using CeciDapper.Infra.CrossCutting.Extensions;
using CeciDapper.Infra.CrossCutting.Helper;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service responsible for authentication and token management.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _jobClient;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="tokenService">The token service.</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="uow">The unit of work.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="jobClient">The background job client.</param>
        /// <param name="logger">The logger.</param>
        public AuthService(ITokenService tokenService,
            IEmailService emailService,
            IUnitOfWork uow,
            IMapper mapper,
            IBackgroundJobClient jobClient,
            ILogger<AuthService> logger)
        {
            _tokenService = tokenService;
            _emailService = emailService;
            _uow = uow;
            _mapper = mapper;
            _jobClient = jobClient;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <param name="ipAddress">The IP address of the user.</param>
        /// <returns>The authentication result.</returns>
        public async Task<ResultResponse<AuthResultDTO>> AuthenticateAsync(LoginDTO model, string ipAddress)
        {
            var result = new ResultResponse<AuthResultDTO>();

            try
            {
                var user = await _uow.User.GetFirstOrDefaultAsync($"{nameof(User.Email)} = '{model.Username}'");

                if (user != null)
                {
                    if (!PasswordExtension.DecryptPassword(user.Password).Equals(model.Password))
                    {
                        result.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                        result.Message = "Password incorret.";
                        return result;
                    };

                    var userValid = await _uow.User.GetUserByIdAsync(user.Id);

                    //authentication successful so generate jwt and refresh tokens
                    var jwtToken = _tokenService.GenerateToken(_mapper.Map<UserResultDTO>(userValid));

                    //generate and save refresh token
                    var refreshToken = GenerateRefreshToken(ipAddress, userValid.Id);
                    await _uow.RefreshToken.AddAsync(refreshToken);

                    result.Data = _mapper.Map<AuthResultDTO>(userValid);
                    result.Data.RefreshToken = refreshToken.Token;
                    result.Data.Token = jwtToken;
                }
                else
                {
                    result.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    result.Message = "User not found";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Unable to authenticate.";
                result.Exception = ex;
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Refreshes the user token.
        /// </summary>
        /// <param name="token">The refresh token.</param>
        /// <param name="ipAddress">The IP address of the user.</param>
        /// <returns>The refreshed authentication result.</returns>
        public async Task<ResultResponse<AuthResultDTO>> RefreshTokenAsync(string token, string ipAddress)
        {
            var result = new ResultResponse<AuthResultDTO>();

            try
            {
                var refreshToken = await _uow.RefreshToken.GetFirstOrDefaultAsync($"{nameof(RefreshToken.Token)} = '{token}'");
                
                if (refreshToken != null && refreshToken.IsActive)
                {
                    // generate new refresh token
                    var newRefreshToken = GenerateRefreshToken(ipAddress, refreshToken.UserId);

                    // replace old refresh token
                    refreshToken.Revoked = DateTime.UtcNow;
                    refreshToken.RevokedByIp = ipAddress;
                    refreshToken.ReplacedByToken = newRefreshToken.Token;
                    await _uow.RefreshToken.UpdateAsync(refreshToken);

                    //save new refresh token
                    await _uow.RefreshToken.AddAsync(newRefreshToken);

                    // generate new jwt token
                    var jwtToken = _tokenService.GenerateToken(_mapper.Map<UserResultDTO>(refreshToken.User));

                    result.Data = _mapper.Map<AuthResultDTO>(refreshToken.User);
                    result.Data.RefreshToken = newRefreshToken.Token;
                    result.Data.Token = jwtToken;
                }
                else
                {
                    result.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    result.Message = "Token not found or expired.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Unable to refresh token.";
                result.Exception = ex;
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Revokes a user token.
        /// </summary>
        /// <param name="token">The token to revoke.</param>
        /// <param name="ipAddress">The IP address of the user.</param>
        /// <returns>A response indicating the success of the revocation.</returns>
        public async Task<ResultResponse> RevokeTokenAsync(string token, string ipAddress)
        {
            var result = new ResultResponse();

            try
            {
                var atualToken = await _uow.RefreshToken.GetFirstOrDefaultAsync($"{nameof(RefreshToken.Token)} = '{token}'" +
                    $" AND {nameof(RefreshToken.IsActive)} = 1");

                if (atualToken != null)
                {
                    atualToken.Revoked = DateTime.UtcNow;
                    atualToken.RevokedByIp = ipAddress;
                    await _uow.RefreshToken.UpdateAsync(atualToken);

                    result.Message = "Token successfully revoked.";
                }
                else
                {
                    result.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    result.Message = "Token not found or expired.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Unable to revoke token.";
                result.Exception = ex;
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Generates a new password and sends it to the user's email.
        /// </summary>
        /// <param name="model">The user's email address.</param>
        /// <returns>A response indicating the success of the password reset request.</returns>
        public async Task<ResultResponse> ForgotPasswordAsync(ForgotPasswordDTO model)
        {
            var result = new ResultResponse();

            try
            {
                var user = await _uow.User.GetFirstOrDefaultAsync($"{nameof(User.Email)} = '{model.Email}'");

                var newPassword = PasswordExtension.GeneratePassword(2, 2, 2, 2);

                user.Password = PasswordExtension.EncryptPassword(StringHelper.Base64Encode(newPassword));

                await _uow.User.UpdateAsync(user);

                _jobClient.Enqueue(() => _emailService.SendEmailAsync(new EmailRequestDTO
                {
                    Body = "A password change request has been requested for your user. Use the password <b>" + newPassword + "</b> in your next application access.",
                    Subject = user.Name,
                    ToEmail = user.Email
                }));

                result.Message = "Request made successfully";

            }
            catch (Exception ex)
            {
                result.Message = "Unable request made.";
                result.Exception = ex;
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        // helper methods
        private RefreshToken GenerateRefreshToken(string ipAddress, int userId)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    RegistrationDate = DateTime.UtcNow,
                    CreatedByIp = ipAddress,
                    UserId = userId
                };
            }
        }
    }
}
