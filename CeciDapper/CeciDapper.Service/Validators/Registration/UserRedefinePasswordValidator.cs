using CeciDapper.Domain.DTO.Register;
using CeciDapper.Infra.CrossCutting.Helper;
using FluentValidation;

namespace CeciDapper.Service.Validators.Registration
{
    /// <summary>
    /// Validator for validating the request to redefine a user's password.
    /// </summary>
    public class UserRedefinePasswordValidator : AbstractValidator<UserRedefinePasswordDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRedefinePasswordValidator"/> class.
        /// </summary>
        public UserRedefinePasswordValidator()
        {
            RuleFor(c => c.CurrentPassword)
                .NotEmpty().WithMessage("Please enter the current password.")
                .NotNull().WithMessage("Please enter the current password.");

            When(c => !string.IsNullOrEmpty(c.CurrentPassword), () =>
            {
                RuleFor(c => c.CurrentPassword)
                    .Must(c => StringHelper.IsBase64String(c))
                    .WithMessage("Password must be base64 encoded.");
            });

            RuleFor(c => c.NewPassword)
                .NotEmpty().WithMessage("Please enter the new password.")
                .NotNull().WithMessage("Please enter the new password.");

            When(c => !string.IsNullOrEmpty(c.NewPassword), () =>
            {
                RuleFor(c => c.NewPassword)
                    .Must(c => StringHelper.IsBase64String(c))
                    .WithMessage("Password must be base64 encoded.");
            });
        }
    }
}
