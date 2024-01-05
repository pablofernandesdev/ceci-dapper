using CeciDapper.Domain.DTO.Register;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Infra.CrossCutting.Helper;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciDapper.Service.Validators.Registration
{
    /// <summary>
    /// Validator for validating the self-registration request of a user.
    /// </summary>
    public class UserSelfRegistrationValidator : AbstractValidator<UserSelfRegistrationDTO>
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSelfRegistrationValidator"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public UserSelfRegistrationValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please enter the name.")
                .NotNull().WithMessage("Please enter the name.");

            RuleFor(c => c.Email)
                .EmailAddress()
                .MustAsync(async (email, cancellation) => {
                    return !await RegisteredEmail(email);
                }).WithMessage("E-mail already registered.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please enter the password.")
                .NotNull().WithMessage("Please enter the password.");

            When(c => !string.IsNullOrEmpty(c.Password), () => {
                RuleFor(c => c.Password)
                    .Must(c => StringHelper.IsBase64String(c))
                    .WithMessage("Password must be base64 encoded.");
            });
        }

        /// <summary>
        /// Checks if the provided email is already registered.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns><c>true</c> if the email is not registered; otherwise, <c>false</c>.</returns>
        private async Task<bool> RegisteredEmail(string email)
        {
            return await _uow.User.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.User.Email)} = '{email}'") != null;
        }
    }
}
