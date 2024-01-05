using System.Collections.Generic;
using System.Threading.Tasks;
using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.User;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for managing user-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a collection of users based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter criteria to apply.</param>
        /// <returns>A task representing the asynchronous operation with the result of user data.</returns>
        Task<ResultDataResponse<IEnumerable<UserResultDTO>>> GetAsync(UserFilterDTO filter);

        /// <summary>
        /// Adds a new user with the provided information.
        /// </summary>
        /// <param name="obj">The DTO containing the user information to be added.</param>
        /// <returns>A task representing the asynchronous operation with the result of the addition.</returns>
        Task<ResultResponse> AddAsync(UserAddDTO obj);

        /// <summary>
        /// Deletes a user based on the provided information.
        /// </summary>
        /// <param name="obj">The DTO containing the user information to be deleted.</param>
        /// <returns>A task representing the asynchronous operation with the result of the deletion.</returns>
        Task<ResultResponse> DeleteAsync(UserDeleteDTO obj);

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="obj">The DTO containing the updated user information.</param>
        /// <returns>A task representing the asynchronous operation with the result of the update.</returns>
        Task<ResultResponse> UpdateAsync(UserUpdateDTO obj);

        /// <summary>
        /// Retrieves a user's information based on their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>A task representing the asynchronous operation with the result of the user data.</returns>
        Task<ResultResponse<UserResultDTO>> GetByIdAsync(int id);

        /// <summary>
        /// Updates the role of a user.
        /// </summary>
        /// <param name="obj">The DTO containing the user's updated role information.</param>
        /// <returns>A task representing the asynchronous operation with the result of the role update.</returns>
        Task<ResultResponse> UpdateRoleAsync(UserUpdateRoleDTO obj);
    }
}
