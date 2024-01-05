using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CeciDapper.Domain.DTO.Commons
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a generic response with a message and details for API operations.
    /// </summary>
    public class ResultResponse
    {
        /// <summary>
        /// Gets or sets the message describing the result of the API operation.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets additional details related to the result of the API operation.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets a value indicating whether the response has a successful status code (2xx).
        /// </summary>
        [JsonIgnore]
        public bool IsSuccessStatusCode => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        private Exception _exception { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code for the response.
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        /// <summary>
        /// Gets or sets the exception that occurred during the API operation, if any.
        /// </summary>
        [JsonIgnore]
        public Exception Exception
        {
            get
            {
                return _exception;
            }
            set
            {
                if (value != null)
                {
                    StatusCode = HttpStatusCode.InternalServerError;
                    Details = value.Message;
                    _exception = value;
                }
            }
        }

        /// <summary>
        /// Returns a JSON string representation of the ResultResponse object.
        /// </summary>
        /// <returns>A JSON string representing the ResultResponse object.</returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    /// <summary>
    /// Data Transfer Object (DTO) representing a generic response with data and additional information for API operations.
    /// </summary>
    /// <typeparam name="TData">The type of data to be included in the response.</typeparam>
    public class ResultResponse<TData> : ResultResponse
    {
        /// <summary>
        /// Gets or sets the data to be included in the response.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TData Data { get; set; }
    }
}