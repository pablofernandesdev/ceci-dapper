using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Repository
{
    /// <summary>
    /// Represents the repository interface for performing CRUD operations on Address entities.
    /// </summary>
    public interface IAddressRepository : IBaseRepository<Address>
    {
        /// <summary>
        /// Retrieves a collection of addresses for the specified logged-in user, based on filter conditions.
        /// </summary>
        /// <param name="userId">The identifier of the logged-in user.</param>
        /// <param name="filter">The filter conditions to apply to the addresses.</param>
        /// <returns>A collection of addresses that match the filter conditions.</returns>
        Task<IEnumerable<Address>> GetLoggedUserAddressesAsync(int userId, AddressFilterDTO filter);

        /// <summary>
        /// Retrieves the total count of addresses for the specified logged-in user, based on filter conditions.
        /// </summary>
        /// <param name="userId">The identifier of the logged-in user.</param>
        /// <param name="filter">The filter conditions to apply to the addresses.</param>
        /// <returns>The total count of addresses that match the filter conditions.</returns>
        Task<int> GetTotalLoggedUserAddressesAsync(int userId, AddressFilterDTO filter);

        /// <summary>
        /// Retrieves a collection of addresses based on filter conditions.
        /// </summary>
        /// <param name="filter">The filter conditions to apply to the addresses.</param>
        /// <returns>A collection of addresses that match the filter conditions.</returns>
        Task<IEnumerable<Address>> GetByFilterAsync(AddressFilterDTO filter);

        /// <summary>
        /// Retrieves the total count of addresses based on filter conditions.
        /// </summary>
        /// <param name="filter">The filter conditions to apply to the addresses.</param>
        /// <returns>The total count of addresses that match the filter conditions.</returns>
        Task<int> GetTotalByFilterAsync(AddressFilterDTO filter);
    }
}
