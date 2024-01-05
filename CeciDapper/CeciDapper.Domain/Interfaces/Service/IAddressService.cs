using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.DTO.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for managing address-related operations.
    /// </summary>
    public interface IAddressService
    {
        /// <summary>
        /// Retrieves address information asynchronously based on a specified zip code.
        /// </summary>
        /// <param name="obj">The DTO containing the zip code to retrieve address information for.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse containing the retrieved address information.</returns>
        Task<ResultResponse<AddressResultDTO>> GetAddressByZipCodeAsync(AddressZipCodeDTO obj);

        /// <summary>
        /// Adds a new address asynchronously.
        /// </summary>
        /// <param name="obj">The DTO containing the address information to add.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the operation.</returns>
        Task<ResultResponse> AddAsync(AddressAddDTO obj);

        /// <summary>
        /// Updates an existing address asynchronously.
        /// </summary>
        /// <param name="obj">The DTO containing the updated address information.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the operation.</returns>
        Task<ResultResponse> UpdateAsync(AddressUpdateDTO obj);

        /// <summary>
        /// Deletes an address asynchronously based on its identifier.
        /// </summary>
        /// <param name="id">The identifier of the address to delete.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the operation.</returns>
        Task<ResultResponse> DeleteAsync(int id);

        /// <summary>
        /// Retrieves a list of addresses asynchronously based on the provided filter.
        /// </summary>
        /// <param name="filter">The DTO containing the filter parameters.</param>
        /// <returns>A task representing the asynchronous operation. A ResultDataResponse containing the retrieved addresses.</returns>
        Task<ResultDataResponse<IEnumerable<AddressResultDTO>>> GetAsync(AddressFilterDTO filter);

        /// <summary>
        /// Retrieves an address by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the address to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse containing the retrieved address.</returns>
        Task<ResultResponse<AddressResultDTO>> GetByIdAsync(int id);
    }
}
