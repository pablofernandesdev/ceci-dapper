using CeciDapper.Domain.DTO.Auth;
using FluentValidation;

namespace CeciDapper.Service.Validators.Login
{
    /// <summary>
    /// Validator for validating the login request.
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginValidator"/> class.
        /// </summary>
        public LoginValidator()
        {
            RuleFor(c => c.Username)
                .NotEmpty().WithMessage("Please enter the username.")
                .NotNull().WithMessage("Please enter the username.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please enter the password.")
                .NotNull().WithMessage("Please enter the password.");
        }
    }
}
