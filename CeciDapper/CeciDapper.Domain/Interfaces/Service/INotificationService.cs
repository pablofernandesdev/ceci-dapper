using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Notification;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for sending notifications.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Sends a notification asynchronously using the provided notification data.
        /// </summary>
        /// <param name="obj">The DTO containing notification information.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the notification sending operation.</returns>
        Task<ResultResponse> SendAsync(NotificationSendDTO obj);
    }
}
