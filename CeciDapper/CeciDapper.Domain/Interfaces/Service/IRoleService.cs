using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Role;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for managing user roles.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Retrieves a collection of user roles.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. A ResultDataResponse containing the collection of role information.</returns>
        Task<ResultDataResponse<IEnumerable<RoleResultDTO>>> GetAsync();

        /// <summary>
        /// Adds a new user role.
        /// </summary>
        /// <param name="obj">The DTO containing the role information to be added.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the result of the addition.</returns>
        Task<ResultResponse> AddAsync(RoleAddDTO obj);

        /// <summary>
        /// Deletes a user role based on its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the role to be deleted.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the result of the deletion.</returns>
        Task<ResultResponse> DeleteAsync(int id);

        /// <summary>
        /// Updates the information of an existing user role.
        /// </summary>
        /// <param name="obj">The DTO containing the updated role information.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the result of the update.</returns>
        Task<ResultResponse> UpdateAsync(RoleUpdateDTO obj);

        /// <summary>
        /// Retrieves detailed information about a user role based on its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the role to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse containing the detailed role information.</returns>
        Task<ResultResponse<RoleResultDTO>> GetByIdAsync(int id);
    }
}
