using CeciDapper.Domain.DTO.Role;
using CeciDapper.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciDapper.Service.Validators.Role
{
    /// <summary>
    /// Validator for updating a role.
    /// </summary>
    public class RoleUpdateValidator : AbstractValidator<RoleUpdateDTO>
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleUpdateValidator"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public RoleUpdateValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.RoleId)
                .NotNull().WithMessage("Please enter the role.")
                .MustAsync(async (roleId, cancellation) => {
                    return await RoleValid(roleId);
                }).WithMessage("Role invalid.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please enter the name.")
                .NotNull().WithMessage("Please enter the name.");
        }

        /// <summary>
        /// Checks if the role is valid.
        /// </summary>
        /// <param name="roleId">The role identifier to validate.</param>
        /// <returns>True if the role is valid, otherwise false.</returns>
        private async Task<bool> RoleValid(int roleId)
        {
            return await _uow.Role.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.Role.Id)} = {roleId}") != null;
        }
    }
}
