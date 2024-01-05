using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciDapper.Service.Validators.User
{
    /// <summary>
    /// Validator for deleting a user.
    /// </summary>
    public class UserDeleteValidator : AbstractValidator<UserDeleteDTO>
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDeleteValidator"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public UserDeleteValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("Please enter the identifier user.")
                .NotNull().WithMessage("Please enter the identifier user.")
                .MustAsync(async (userId, cancellation) => {
                    return await UserValid(userId);
                }).WithMessage("User invalid.");
        }

        /// <summary>
        /// Checks if the user is valid.
        /// </summary>
        /// <param name="userId">The user identifier to validate.</param>
        /// <returns>True if the user is valid, otherwise false.</returns>
        private async Task<bool> UserValid(int userId)
        {
            return await _uow.User.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.User.Id)} = {userId}") != null;
        }
    }
}
