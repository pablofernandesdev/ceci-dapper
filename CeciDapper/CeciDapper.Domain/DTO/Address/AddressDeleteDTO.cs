using Microsoft.AspNetCore.Mvc;

namespace CeciDapper.Domain.DTO.Address
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the information for deleting an address.
    /// </summary>
    public class AddressDeleteDTO
    {
        /// <summary>
        /// Gets or sets the identifier of the address to be deleted.
        /// </summary>
        /// <remarks>
        /// This property is bound to the "addressId" parameter in the HTTP request using the [BindProperty] attribute.
        /// </remarks>
        [BindProperty(Name = "addressId")]
        public int AddressId { get; set; }
    }
}
