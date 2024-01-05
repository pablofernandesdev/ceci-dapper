namespace CeciDapper.Domain.DTO.Register
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the address information to be updated for a logged-in user.
    /// </summary>
    public class AddressLoggedUserUpdateDTO
    {
        /// <summary>
        /// Gets or sets the identifier of the address to be updated.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Gets or sets the zip code of the address.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the street name of the address.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the district or neighborhood name of the address.
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Gets or sets the locality or city name of the address.
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
        /// Gets or sets the state or UF (Unit Federative) of the address.
        /// </summary>
        public string Uf { get; set; }
    }
}