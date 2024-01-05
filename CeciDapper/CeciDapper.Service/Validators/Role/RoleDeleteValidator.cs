using CeciDapper.Domain.DTO.Role;
using CeciDapper.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciDapper.Service.Validators.Role
{
    /// <summary>
    /// Validator for deleting a role.
    /// </summary>
    public class RoleDeleteValidator : AbstractValidator<RoleDeleteDTO>
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDeleteValidator"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public RoleDeleteValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.RoleId)
                .NotEmpty().WithMessage("Please enter the identifier role.")
                .NotNull().WithMessage("Please enter the identifier role.")
                .MustAsync(async (roleId, cancellation) => {
                    return await RoleValid(roleId);
                }).WithMessage("Role invalid.");
        }

        /// <summary>
        /// Checks if the role with the given identifier is valid.
        /// </summary>
        /// <param name="roleId">The role identifier to validate.</param>
        /// <returns>Returns true if the role is valid; otherwise, false.</returns>
        private async Task<bool> RoleValid(int roleId)
        {
            return await _uow.Role.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.Role.Id)} = {roleId}") != null;
        }
    }
}
