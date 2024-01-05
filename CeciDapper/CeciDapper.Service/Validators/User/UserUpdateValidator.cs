using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciDapper.Service.Validators.User
{
    /// <summary>
    /// Validator for updating a user.
    /// </summary>
    public class UserUpdateValidator : AbstractValidator<UserUpdateDTO>
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserUpdateValidator"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public UserUpdateValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("Please enter the identifier user.")
                .NotNull().WithMessage("Please enter the identifier user.")
                .MustAsync(async (userId, cancellation) => {
                    return await UserValid(userId);
                }).WithMessage("User invalid.");

            RuleFor(c => c.RoleId)
                .NotNull().WithMessage("Please enter the role.")
                .MustAsync(async (roleId, cancellation) => {
                    return await RoleValid(roleId);
                }).WithMessage("Role invalid.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please enter the name.")
                .NotNull().WithMessage("Please enter the name.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Please enter the email user.")
                .NotNull().WithMessage("Please enter the email user.")
                .EmailAddress().WithMessage("Please enter the valid email user.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please enter the password.")
                .NotNull().WithMessage("Please enter the password.");
        }

        /// <summary>
        /// Validates if the provided role ID is valid.
        /// </summary>
        /// <param name="roleId">The role ID.</param>
        /// <returns>Returns true if the role is valid; otherwise, false.</returns>
        private async Task<bool> RoleValid(int roleId)
        {
            return await _uow.Role.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.Role.Id)} = {roleId}") != null;
        }

        /// <summary>
        /// Validates if the provided user ID is valid.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Returns true if the user is valid; otherwise, false.</returns>
        private async Task<bool> UserValid(int userId)
        {
            return await _uow.User.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.User.Id)} = {userId}") != null;
        }
    }
}
