namespace CeciDapper.Domain.DTO.Address
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the information for adding an address.
    /// </summary>
    public class AddressAddDTO
    {
        /// <summary>
        /// Gets or sets the user ID associated with the address.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the zip code of the address.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the street name of the address.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the district name of the address.
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Gets or sets the locality (city or town) of the address.
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets the house or building number of the address.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets additional information or complement for the address.
        /// </summary>
        public string Complement { get; set; }

        /// <summary>
        /// Gets or sets the state abbreviation (UF - Unidade Federativa) of the address.
        /// </summary>
        public string Uf { get; set; }
    }
}
