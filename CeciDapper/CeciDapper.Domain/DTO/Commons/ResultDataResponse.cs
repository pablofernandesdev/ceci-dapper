using System;
using System.Net;
using System.Text.Json.Serialization;

namespace CeciDapper.Domain.DTO.Commons
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a response with data and additional information for API operations.
    /// </summary>
    /// <typeparam name="TData">The type of data to be included in the response.</typeparam>
    public class ResultDataResponse<TData>
    {
        /// <summary>
        /// Gets or sets the total number of pages in the data collection.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total number of items in the data collection.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the data to be included in the response.
        /// </summary>
        public TData Data { get; set; }

        private Exception _exception { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code for the response.
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        /// <summary>
        /// Gets or sets the exception that occurred during the API operation.
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
                    _exception = value;
                }
            }
        }
    }
}