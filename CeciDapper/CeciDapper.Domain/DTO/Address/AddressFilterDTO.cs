using CeciDapper.Domain.DTO.Commons;

namespace CeciDapper.Domain.DTO.Address
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the filter criteria for querying addresses.
    /// </summary>
    public class AddressFilterDTO : QueryFilter
    {
        /// <summary>
        /// Gets or sets the district name for filtering addresses.
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Gets or sets the locality (city or town) for filtering addresses.
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets the state abbreviation (UF - Unidade Federativa) for filtering addresses.
        /// </summary>
        public string Uf { get; set; }
    }
}
