using CeciDapper.Domain.DTO.User;
using FluentValidation;

namespace CeciDapper.Service.Validators.User
{
    /// <summary>
    /// Validator for validating a user identifier.
    /// </summary>
    public class UserIdentifierValidator : AbstractValidator<UserIdentifierDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserIdentifierValidator"/> class.
        /// </summary>
        public UserIdentifierValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("Please enter the identifier user.")
                .NotNull().WithMessage("Please enter the identifier user.");
        }
    }
}
