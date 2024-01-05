using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciDapper.Service.Validators.Address
{
    /// <summary>
    /// Validator for validating the address update request.
    /// </summary>
    public class AddressUpdateValidator : AbstractValidator<AddressUpdateDTO>
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressUpdateValidator"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public AddressUpdateValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.AddressId)
                .NotEmpty().WithMessage("Please enter the address id.")
                .NotNull().WithMessage("Please enter the address id.")
                .MustAsync(async (addressId, cancellation) => {
                    return await AddressValid(addressId);
                }).WithMessage("Address invalid.");

            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("Please enter the identifier user.")
                .NotNull().WithMessage("Please enter the identifier user.")
                .MustAsync(async (userId, cancellation) => {
                    return await UserValid(userId);
                }).WithMessage("User invalid.");

            RuleFor(c => c.ZipCode)
                .NotEmpty().WithMessage("Please enter the zip code.")
                .NotNull().WithMessage("Please enter the zip code.");

            RuleFor(c => c.Street)
                .NotEmpty().WithMessage("Please enter the street.")
                .NotNull().WithMessage("Please enter the street.");

            RuleFor(c => c.District)
                 .NotEmpty().WithMessage("Please enter the district.")
                 .NotNull().WithMessage("Please enter the district.");

            RuleFor(c => c.Locality)
                 .NotEmpty().WithMessage("Please enter the locality.")
                 .NotNull().WithMessage("Please enter the locality.");

            RuleFor(c => c.Number)
                 .NotEmpty().WithMessage("Please enter the number.")
                 .NotNull().WithMessage("Please enter the number.");

            RuleFor(c => c.Uf)
                 .NotEmpty().WithMessage("Please enter the uf.")
                 .NotNull().WithMessage("Please enter the uf.");
        }

        private async Task<bool> AddressValid(int addressId)
        {
            return await _uow.Address.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.Address.Id)} = {addressId}") != null;
        }

        private async Task<bool> UserValid(int userId)
        {
            return await _uow.User.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.User.Id)} = {userId}") != null;
        }
    }
}
