using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciDapper.Service.Validators.Address
{
    /// <summary>
    /// Validator for validating the data when deleting an address.
    /// </summary>
    public class AddressDeleteValidator : AbstractValidator<AddressDeleteDTO>
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressDeleteValidator"/> class.
        /// </summary>
        /// <param name="uow">The <see cref="IUnitOfWork"/> instance used to access database operations.</param>
        public AddressDeleteValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.AddressId)
                .NotEmpty().WithMessage("Please enter the address id.")
                .NotNull().WithMessage("Please enter the address id.")
                .MustAsync(async (addressId, cancellation) => {
                    return await AddressValid(addressId);
                }).WithMessage("Address invalid.");           
        }

        private async Task<bool> AddressValid(int addressId)
        {
            return await _uow.Address.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.Address.Id)} = {addressId}") != null;
        }
    }
}
