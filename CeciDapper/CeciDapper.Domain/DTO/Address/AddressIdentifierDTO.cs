using Microsoft.AspNetCore.Mvc;

namespace CeciDapper.Domain.DTO.Address
{
    /// <summary>
    /// Data Transfer Object (DTO) representing an identifier for an address.
    /// </summary>
    public class AddressIdentifierDTO
    {
        /// <summary>
        /// Gets or sets the identifier of the address.
        /// </summary>
        /// <remarks>
        /// This property is bound to the "addressId" parameter in the HTTP request using the [BindProperty] attribute.
        /// </remarks>
        [BindProperty(Name = "addressId")]
        public int AddressId { get; set; }
    }
}
