using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Email;
using CeciDapper.Domain.DTO.ValidationCode;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using CeciDapper.Infra.CrossCutting.Extensions;
using CeciDapper.Infra.CrossCutting.Helper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service responsible for managing the logic of generating and validating validation codes.
    /// </summary>
    public class ValidationCodeService : IValidationCodeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobClient _jobClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationCodeService"/> class.
        /// </summary>
        /// <param name="uow">The <see cref="IUnitOfWork"/> instance used to access database operations.</param>
        /// <param name="emailService">The <see cref="IEmailService"/> instance used to send emails.</param>
        /// <param name="jobClient">The <see cref="IBackgroundJobClient"/> instance used to enqueue background jobs.</param>
        /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance used to access the current HTTP context.</param>
        public ValidationCodeService(IUnitOfWork uow,
            IEmailService emailService,
            IBackgroundJobClient jobClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _emailService = emailService;
            _jobClient = jobClient;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Sends a validation code to the currently authenticated user.
        /// </summary>
        /// <returns>A <see cref="ResultResponse"/> object indicating the result of the operation.</returns>
        public async Task<ResultResponse> SendAsync()
        {
            var response = new ResultResponse();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var user = await _uow.User
                    .GetUserByIdAsync(Convert.ToInt32(userId));

                var code = PasswordExtension.GeneratePassword(0, 0, 6, 0);
                
                await _uow.ValidationCode.AddAsync(new ValidationCode {
                    Code = PasswordExtension.EncryptPassword(StringHelper.Base64Encode(code)),
                    UserId = user.Id,
                    Expires = System.DateTime.UtcNow.AddMinutes(10)
                });

                user.Validated = false;
                await _uow.User.UpdateAsync(user);

                response.Message = "Code sent successfully.";

                EnqueueEmailSending(user.Name, user.Email, code);
            }
            catch (Exception ex)
            {
                response.Message = "Could not send code.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Validates a validation code provided by the user.
        /// </summary>
        /// <param name="obj">A <see cref="ValidationCodeValidateDTO"/> object containing the validation code to be validated.</param>
        /// <returns>A <see cref="ResultResponse"/> object indicating the result of the operation.</returns>
        public async Task<ResultResponse> ValidateCodeAsync(ValidationCodeValidateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();
                int parsedUserId = Convert.ToInt32(userId);

                var validationCode = await _uow.ValidationCode
                    .GetFirstOrDefaultAsync($"{nameof(ValidationCode.UserId)} = {parsedUserId}");

                if (validationCode == null || !IsValidCode(validationCode, obj.Code))
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = "Invalid or expired validation code.";
                    return response;
                }

                var user = await _uow.User
                    .GetUserByIdAsync(parsedUserId);

                user.Validated = true;
                await _uow.User.UpdateAsync(user);

                response.Message = "Code validated successfully.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not validate code.";
                response.Exception = ex;
            }

            return response;
        }

        private bool IsValidCode(ValidationCode validationCode, string code)
        {
            return PasswordExtension.DecryptPassword(validationCode.Code).Equals(code)
                   && !validationCode.IsExpired;
        }

        private void EnqueueEmailSending(string userName, string userEmail, string code)
        {
            _jobClient.Enqueue(() => _emailService.SendEmailAsync(new EmailRequestDTO
            {
                Body = $"A new validation code was requested. Use the code <b>{code}</b> to complete validation.",
                Subject = userName,
                ToEmail = userEmail
            }));
        }
    }
}
