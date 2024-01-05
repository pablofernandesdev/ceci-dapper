using CeciDapper.Domain.DTO.Role;
using FluentValidation;

namespace CeciDapper.Service.Validators.Role
{
    /// <summary>
    /// Validator for role identifier.
    /// </summary>
    public class RoleIdentifierValidator : AbstractValidator<IdentifierRoleDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleIdentifierValidator"/> class.
        /// </summary>
        public RoleIdentifierValidator()
        {
            RuleFor(c => c.RoleId)
                .NotEmpty().WithMessage("Please enter the identifier role.")
                .NotNull().WithMessage("Please enter the identifier role.");
        }
    }
}
