using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.Interfaces.Service.External;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services.External
{
    /// <summary>
    /// Service responsible for sending notifications through Firebase Cloud Messaging (FCM).
    /// </summary>
    public class FirebaseService : IFirebaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the FirebaseService class.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client used to send requests to FCM.</param>
        public FirebaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Sends an asynchronous notification to a specific device identified by its token.
        /// </summary>
        /// <param name="token">Target device token.</param>
        /// <param name="title">Notification title.</param>
        /// <param name="body">Notification body.</param>
        /// <returns>ResultResponse object containing the operation status.</returns>
        public async Task<ResultResponse> SendNotificationAsync(string token, string title, string body)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var response = new ResultResponse();

                // Constructs the data object to be sent to FCM
                var data = new
                {
                    to = token,
                    notification = new
                    {
                        body,
                        title,
                    },
                    priority = "high"
                };

                // Serializes the object into JSON format
                var json = JsonConvert.SerializeObject(data);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    // Sends the POST request to FCM
                    var result = await httpClient.PostAsync("/fcm/send", httpContent);

                    // Sets the response with the status code returned by the request
                    response.StatusCode = result.StatusCode;

                    // If the request was not successful, sets the error message
                    if (!result.IsSuccessStatusCode)
                    {
                        response.Message = await result.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Message = $"An error occurred while sending the notification: {ex.Message}";
                }

                return response;
            }
        }
    }
}
