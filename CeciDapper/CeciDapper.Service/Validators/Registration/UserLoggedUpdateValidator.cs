using CeciDapper.Domain.DTO.Register;
using FluentValidation;

namespace CeciDapper.Service.Validators.Registration
{
    /// <summary>
    /// Validator for validating the update request of a logged-in user.
    /// </summary>
    public class UserLoggedUpdateValidator : AbstractValidator<UserLoggedUpdateDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoggedUpdateValidator"/> class.
        /// </summary>
        public UserLoggedUpdateValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please enter the name.")
                .NotNull().WithMessage("Please enter the name.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Please enter the email user.")
                .NotNull().WithMessage("Please enter the email user.")
                .EmailAddress().WithMessage("Please enter the valid email user.");
        }
    }
}
