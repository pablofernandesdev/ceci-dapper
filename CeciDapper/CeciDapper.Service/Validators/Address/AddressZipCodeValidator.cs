using CeciDapper.Domain.DTO.Address;
using FluentValidation;

namespace CeciDapper.Service.Validators.Address
{
    /// <summary>
    /// Validator for validating the address zip code request.
    /// </summary>
    public class AddressZipCodeValidator : AbstractValidator<AddressZipCodeDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressZipCodeValidator"/> class.
        /// </summary>
        public AddressZipCodeValidator()
        {
            RuleFor(c => c.ZipCode)
                .NotEmpty().WithMessage("Please enter the zip code.")
                .NotNull().WithMessage("Please enter the zip code.");
        }
    }
}
