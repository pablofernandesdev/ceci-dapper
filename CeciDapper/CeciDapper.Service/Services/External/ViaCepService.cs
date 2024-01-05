using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.ViaCep;
using CeciDapper.Domain.Interfaces.Service.External;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services.External
{
    /// <summary>
    /// Service responsible for retrieving address information using the ViaCep API.
    /// </summary>
    public class ViaCepService : IViaCepService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the ViaCepService class.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client used to send requests to the ViaCep API.</param>
        public ViaCepService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Retrieves the address information associated with a given ZIP code asynchronously.
        /// </summary>
        /// <param name="zipCode">ZIP code to retrieve the address for.</param>
        /// <returns>ResultResponse object containing the operation status and address information.</returns>
        public async Task<ResultResponse<ViaCepAddressResponseDTO>> GetAddressByZipCodeAsync(string zipCode)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var response = new ResultResponse<ViaCepAddressResponseDTO>();

                try
                {
                    var requestApi = await httpClient.GetAsync($"/ws/{zipCode}/json/");

                    response.StatusCode = requestApi.StatusCode;

                    var dataRequest = await requestApi.Content.ReadAsStringAsync();

                    if (!requestApi.IsSuccessStatusCode)
                    {
                        response.Message = dataRequest;
                        return response;
                    }

                    response.Data = JsonConvert.DeserializeObject<ViaCepAddressResponseDTO>(dataRequest);
                }
                catch (System.Exception)
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Message = "An error occurred while retrieving the address information.";
                }

                return response;
            }
        }
    }
}