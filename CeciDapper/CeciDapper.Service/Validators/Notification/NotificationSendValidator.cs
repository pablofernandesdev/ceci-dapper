using CeciDapper.Domain.DTO.Notification;
using CeciDapper.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciDapper.Service.Validators.Notification
{
    /// <summary>
    /// Validator for validating the notification send request.
    /// </summary>
    public class NotificationSendValidator : AbstractValidator<NotificationSendDTO>
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSendValidator"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public NotificationSendValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.IdUser)
              .NotNull().WithMessage("Please enter the user identifier.")
              .MustAsync(async (userId, cancellation) => {
                  return await UserValid(userId);
              }).WithMessage("User invalid.");

            RuleFor(c => c.Title)
               .NotEmpty().WithMessage("Please enter the notification title.")
               .NotNull().WithMessage("Please enter the notification title.");

            RuleFor(c => c.Body)
               .NotEmpty().WithMessage("Please enter the notification body.")
               .NotNull().WithMessage("Please enter the notification body.");
        }

        /// <summary>
        /// Validates if the user is valid based on the provided user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>True if the user is valid; otherwise, false.</returns>
        private async Task<bool> UserValid(int userId)
        {
            return await _uow.User.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.User.Id)} = {userId}") != null;
        }
    }
}
