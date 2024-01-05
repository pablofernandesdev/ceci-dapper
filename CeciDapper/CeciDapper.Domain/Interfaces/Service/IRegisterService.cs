using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Register;
using CeciDapper.Domain.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for user registration and related operations.
    /// </summary>
    public interface IRegisterService
    {
        /// <summary>
        /// Retrieves the information of the currently logged-in user.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. A ResultResponse containing user information.</returns>
        Task<ResultResponse<UserResultDTO>> GetLoggedInUserAsync();

        /// <summary>
        /// Initiates a self-registration process for a new user.
        /// </summary>
        /// <param name="obj">The DTO containing self-registration information.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the self-registration process.</returns>
        Task<ResultResponse> SelfRegistrationAsync(UserSelfRegistrationDTO obj);

        /// <summary>
        /// Updates the information of the currently logged-in user.
        /// </summary>
        /// <param name="obj">The DTO containing updated user information.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the user information update.</returns>
        Task<ResultResponse> UpdateLoggedUserAsync(UserLoggedUpdateDTO obj);

        /// <summary>
        /// Initiates a password redefinition process for the currently logged-in user.
        /// </summary>
        /// <param name="obj">The DTO containing password redefinition information.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the password redefinition process.</returns>
        Task<ResultResponse> RedefinePasswordAsync(UserRedefinePasswordDTO obj);

        /// <summary>
        /// Adds an address to the currently logged-in user's profile.
        /// </summary>
        /// <param name="obj">The DTO containing address information to be added.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the address addition.</returns>
        Task<ResultResponse> AddLoggedUserAddressAsync(AddressLoggedUserAddDTO obj);

        /// <summary>
        /// Updates an address in the currently logged-in user's profile.
        /// </summary>
        /// <param name="obj">The DTO containing updated address information.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the address update.</returns>
        Task<ResultResponse> UpdateLoggedUserAddressAsync(AddressLoggedUserUpdateDTO obj);

        /// <summary>
        /// Inactivates an address in the currently logged-in user's profile.
        /// </summary>
        /// <param name="obj">The DTO containing information about the address to be inactivated.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the address inactivation.</returns>
        Task<ResultResponse> InactivateLoggedUserAddressAsync(AddressDeleteDTO obj);

        /// <summary>
        /// Retrieves a list of addresses associated with the currently logged-in user's profile.
        /// </summary>
        /// <param name="filter">The DTO containing filtering criteria for retrieving addresses.</param>
        /// <returns>A task representing the asynchronous operation. A ResultDataResponse containing a list of addresses.</returns>
        Task<ResultDataResponse<IEnumerable<AddressResultDTO>>> GetLoggedUserAddressesAsync(AddressFilterDTO filter);

        /// <summary>
        /// Retrieves detailed information about a specific address associated with the currently logged-in user's profile.
        /// </summary>
        /// <param name="obj">The DTO containing identifier information for the address.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse containing detailed address information.</returns>
        Task<ResultResponse<AddressResultDTO>> GetLoggedUserAddressAsync(AddressIdentifierDTO obj);
    }
}
