using CeciDapper.Domain.DTO.ValidationCode;
using FluentValidation;

namespace CeciDapper.Service.Validators.ValidationCode
{
    /// <summary>
    /// Validator for validating a validation code.
    /// </summary>
    public class ValidationCodeValidateValidator : AbstractValidator<ValidationCodeValidateDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationCodeValidateValidator"/> class.
        /// </summary>
        public ValidationCodeValidateValidator()
        {
            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("Please enter the code.")
                .NotNull().WithMessage("Please enter the code.");
        }
    }
}