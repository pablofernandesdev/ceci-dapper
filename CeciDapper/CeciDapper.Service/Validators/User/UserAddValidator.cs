using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Infra.CrossCutting.Helper;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciDapper.Service.Validators.User
{
    /// <summary>
    /// Validator for adding a user.
    /// </summary>
    public class UserAddValidator : AbstractValidator<UserAddDTO>
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAddValidator"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public UserAddValidator(IUnitOfWork uow)
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

            RuleFor(c => c.Email)
                .EmailAddress()
                .MustAsync(async (email, cancellation) => {
                    return !await RegisteredEmail(email);
                }).WithMessage("E-mail already registered.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please enter the password.")
                .NotNull().WithMessage("Please enter the password.");

            When(c => !string.IsNullOrEmpty(c.Password), () => {
                RuleFor(c => c.Password)
                    .Must(c => StringHelper.IsBase64String(c))
                    .WithMessage("Password must be base64 encoded.");
            });
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

        /// <summary>
        /// Checks if the email is already registered.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email is not registered, otherwise false.</returns>
        private async Task<bool> RegisteredEmail(string email)
        {
            return await _uow.User.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.User.Email)} = '{email}'") != null;
        }
    }
}
