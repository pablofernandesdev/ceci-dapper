using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Notification;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using CeciDapper.Domain.Interfaces.Service.External;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service responsible for sending notifications.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFirebaseService _firebaseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        /// <param name="firebaseService">The Firebase service.</param>
        public NotificationService(IUnitOfWork uow,
            IFirebaseService firebaseService)
        {
            _uow = uow;
            _firebaseService = firebaseService;
        }

        /// <summary>
        /// Sends a notification asynchronously.
        /// </summary>
        /// <param name="obj">The notification information.</param>
        /// <returns>A response indicating the success of the notification sending operation.</returns>
        public async Task<ResultResponse> SendAsync(NotificationSendDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var user = await _uow.User.GetFirstOrDefaultAsync($"{nameof(User.Id)} = {obj.IdUser}");

                if (user != null)
                {
                    var registrationToken = await _uow.RegistrationToken.GetFirstOrDefaultAsync($"{nameof(RegistrationToken.UserId)} = {obj.IdUser}");

                    if (registrationToken != null)
                    {
                        response = await _firebaseService.SendNotificationAsync(registrationToken.Token, obj.Title, obj.Body);

                        if (response.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            response.Message = "Notification sent successfully.";
                        }
                    }
                }
             
            }
            catch (Exception ex)
            {
                response.Message = "Could not send notification.";
                response.Exception = ex;
            }

            return response;
        }
    }
}
