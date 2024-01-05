using CeciDapper.Domain.DTO.Commons;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service.External
{
    /// <summary>
    /// Represents a service interface for interacting with the Firebase Cloud Messaging (FCM) service.
    /// </summary>
    public interface IFirebaseService
    {
        /// <summary>
        /// Sends a notification asynchronously to the specified device token.
        /// </summary>
        /// <param name="token">The device token to which the notification will be sent.</param>
        /// <param name="title">The title of the notification.</param>
        /// <param name="body">The body content of the notification.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the success or failure of the notification sending.</returns>
        Task<ResultResponse> SendNotificationAsync(string token, string title, string body);
    }
}
