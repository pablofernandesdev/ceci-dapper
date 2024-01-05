using CeciDapper.Domain.DTO.Address;
using FluentValidation;

namespace CeciDapper.Service.Validators.Address
{
    /// <summary>
    /// Validator for validating the address identifier.
    /// </summary>
    public class AddressIdentifierValidator : AbstractValidator<AddressIdentifierDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressIdentifierValidator"/> class.
        /// </summary>
        public AddressIdentifierValidator()
        {
            RuleFor(c => c.AddressId)
                .NotEmpty().WithMessage("Please enter the address id.")
                .NotNull().WithMessage("Please enter the address id.");
        }
    }
}
