using Newtonsoft.Json;

namespace CeciDapper.Domain.DTO.Address
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the result of an address query.
    /// </summary>
    public class AddressResultDTO
    {
        /// <summary>
        /// Gets or sets the zip code of the address.
        /// </summary>
        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the street name of the address.
        /// </summary>
        [JsonProperty("street")]
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the district name of the address.
        /// </summary>
        [JsonProperty("district")]
        public string District { get; set; }

        /// <summary>
        /// Gets or sets the locality (city or town) of the address.
        /// </summary>
        [JsonProperty("locality")]
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets the state abbreviation (UF - Unidade Federativa) of the address.
        /// </summary>
        [JsonProperty("uf")]
        public string Uf { get; set; }

        /// <summary>
        /// Gets or sets additional information or complement for the address.
        /// </summary>
        [JsonProperty("complement")]
        public string Complement { get; set; }

        /// <summary>
        /// Gets or sets the house or building number of the address.
        /// </summary>
        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
