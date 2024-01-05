using Newtonsoft.Json;

namespace CeciDapper.Domain.DTO.ViaCep
{
    /// <summary>
    /// Data Transfer Object (DTO) representing address information retrieved from the ViaCep API.
    /// </summary>
    public class ViaCepAddressResponseDTO
    {
        /// <summary>
        /// Gets or sets the postal code (CEP) of the address.
        /// </summary>
        [JsonProperty("cep")]
        public string Cep { get; set; }

        /// <summary>
        /// Gets or sets the street name (logradouro) of the address.
        /// </summary>
        [JsonProperty("logradouro")]
        public string Logradouro { get; set; }

        /// <summary>
        /// Gets or sets the address complement (complemento) of the address.
        /// </summary>
        [JsonProperty("complemento")]
        public string Complemento { get; set; }

        /// <summary>
        /// Gets or sets the neighborhood (bairro) of the address.
        /// </summary>
        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        /// <summary>
        /// Gets or sets the city name (localidade) of the address.
        /// </summary>
        [JsonProperty("localidade")]
        public string Localidade { get; set; }

        /// <summary>
        /// Gets or sets the state abbreviation (UF) of the address.
        /// </summary>
        [JsonProperty("uf")]
        public string Uf { get; set; }

        /// <summary>
        /// Gets or sets the IBGE code (ibge) of the address.
        /// </summary>
        [JsonProperty("ibge")]
        public string Ibge { get; set; }

        /// <summary>
        /// Gets or sets the GIA code (gia) of the address.
        /// </summary>
        [JsonProperty("gia")]
        public string Gia { get; set; }

        /// <summary>
        /// Gets or sets the area code (DDD) of the address.
        /// </summary>
        [JsonProperty("ddd")]
        public string Ddd { get; set; }

        /// <summary>
        /// Gets or sets the SIAFI code (siafi) of the address.
        /// </summary>
        [JsonProperty("siafi")]
        public string Siafi { get; set; }
    }
}
