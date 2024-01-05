using CeciDapper.Domain.DTO.Auth;
using CeciDapper.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciDapper.Service.Validators.Login
{
    /// <summary>
    /// Validator for validating the forgot password request.
    /// </summary>
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDTO>
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordValidator"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public ForgotPasswordValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.Email)
                .EmailAddress()
                .MustAsync(async (email, cancellation) => {
                    return await RegisteredEmail(email);
                }).WithMessage("E-mail not found.");
        }

        /// <summary>
        /// Checks if the provided email is registered.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email is registered; otherwise, false.</returns>
        private async Task<bool> RegisteredEmail(string email)
        {
            return await _uow.User.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.User.Email)} = '{email}'") != null;
        }
    }
}
