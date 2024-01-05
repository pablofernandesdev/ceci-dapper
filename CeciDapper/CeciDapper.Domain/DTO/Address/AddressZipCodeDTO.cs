using Microsoft.AspNetCore.Mvc;

namespace CeciDapper.Domain.DTO.Address
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a zip code for address operations.
    /// </summary>
    public class AddressZipCodeDTO
    {
        /// <summary>
        /// Gets or sets the zip code value.
        /// </summary>
        /// <remarks>
        /// This property is bound to the "zipCode" parameter in the HTTP request using the [BindProperty] attribute.
        /// </remarks>
        [BindProperty(Name = "zipCode")]
        public string ZipCode { get; set; }
    }
}
