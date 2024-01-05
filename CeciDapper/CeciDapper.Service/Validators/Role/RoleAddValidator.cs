using CeciDapper.Domain.DTO.Role;
using FluentValidation;

namespace CeciDapper.Service.Validators.Role
{
    /// <summary>
    /// Validator for adding a new role.
    /// </summary>
    public class RoleAddValidator : AbstractValidator<RoleAddDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAddValidator"/> class.
        /// </summary>
        public RoleAddValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please enter the name role.")
                .NotNull().WithMessage("Please enter the name role.");
        }
    }
}