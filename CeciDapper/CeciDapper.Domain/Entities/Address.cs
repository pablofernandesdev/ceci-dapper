namespace CeciDapper.Domain.Entities
{
    /// <summary>
    /// Represents an address entity.
    /// </summary>
    public class Address : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the associated user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the associated user for this address.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the ZIP code of the address.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the street name of the address.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the district (neighborhood) of the address.
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Gets or sets the city or locality of the address.
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets the house or building number of the address.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets additional address information or complement.
        /// </summary>
        public string Complement { get; set; }

        /// <summary>
        /// Gets or sets the state abbreviation (UF) of the address.
        /// </summary>
        public string Uf { get; set; }
    }
}
