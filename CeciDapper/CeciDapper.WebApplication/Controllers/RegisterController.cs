using CeciDapper.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.DTO.Commons;
using Microsoft.AspNetCore.Authorization;
using CeciDapper.Domain.DTO.Register;
using CeciDapper.Domain.DTO.Address;
using System.Collections.Generic;

namespace CeciDapper.WebApplication.Controllers
{
    /// <summary>
    /// Controller responsible for user registration operations.
    /// </summary>
    [Route("api/register")]
    [ApiController]
    [Authorize(Policy = "Basic")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterController"/> class.
        /// </summary>
        /// <param name="registerService">An instance of the register service.</param>
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }


        /// <summary>
        /// Performs self-registration of a user.
        /// </summary>
        /// <param name="model">User data for self-registration.</param>
        /// <returns>Result of the self-registration operation.</returns>
        /// <response code="200">Returns success when creating a new item</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [AllowAnonymous]
        [HttpPost]
        [Route("self-registration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> SelfRegistration([FromBody] UserSelfRegistrationDTO model)
        {
            var result = await _registerService.SelfRegistrationAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Updates the logged-in user.
        /// </summary>
        /// <param name="model">Updated user data.</param>
        /// <returns>Result of the update operation.</returns>
        /// <response code="200">Returns success when updating user logged</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPut]
        [Route("logged-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> UpdateLoggedInUser([FromBody] UserLoggedUpdateDTO model)
        {
            var result = await _registerService.UpdateLoggedUserAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves the logged-in user.
        /// </summary>
        /// <returns>Result containing the logged-in user information.</returns>
        /// <response code="200">Returns success when get logged in user</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [Route("logged-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<UserResultDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<UserResultDTO>>> GetLoggedInUser()
        {
            var result = await _registerService.GetLoggedInUserAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Redefines the password for the user.
        /// </summary>
        /// <param name="model">Data for password redefinition.</param>
        /// <returns>Result of the password redefinition operation.</returns>
        /// <response code="200">Returns success when redefine user password</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("redefine-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> RedefinePassword([FromBody] UserRedefinePasswordDTO model)
        {
            var result = await _registerService.RedefinePasswordAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Adds an address for the logged-in user.
        /// </summary>
        /// <param name="model">The address data to be added.</param>
        /// <returns>An action result containing the add address result.</returns>
        /// <response code="200">Returns success when add logged user address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("logged-user-address")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> AddLoggedInUserAddressAsync([FromBody] AddressLoggedUserAddDTO model)
        {
            var result = await _registerService.AddLoggedUserAddressAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Updates the address for the logged-in user.
        /// </summary>
        /// <param name="model">The updated address data.</param>
        /// <returns>An action result containing the update address result.</returns>
        /// <response code="200">Returns success when updating logged user address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPut]
        [Route("logged-user-address")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> UpdateLoggedInUserAddress([FromBody] UserLoggedUpdateDTO model)
        {
            var result = await _registerService.UpdateLoggedUserAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Deletes the specified address for the logged-in user.
        /// </summary>
        /// <param name="model">The address data to be deleted.</param>
        /// <returns>An action result containing the delete address result.</returns>
        /// <response code="200">Returns success when deleting logged user address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpDelete]
        [Route("logged-user-address/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> DeleteLoggedInUserAddress([FromRoute] AddressDeleteDTO model)
        {
            var result = await _registerService.InactivateLoggedUserAddressAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves all addresses for the logged-in user.
        /// </summary>
        /// <param name="model">The address filter data.</param>
        /// <returns>An action result containing the user addresses.</returns>
        /// <response code="200">Returns success when get logged in user addresses</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [Route("logged-user-address")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDataResponse<IEnumerable<AddressResultDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultDataResponse<IEnumerable<AddressResultDTO>>>> GetLoggedInUserAddresss([FromRoute] AddressFilterDTO model)
        {
            var result = await _registerService.GetLoggedUserAddressesAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves a specific address for the logged-in user.
        /// </summary>
        /// <param name="model">The address identifier data.</param>
        /// <returns>An action result containing the user address.</returns>
        /// <response code="200">Returns success when get logged in user address</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [Route("logged-user-address/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<AddressResultDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<AddressResultDTO>>> GetLoggedInUserAddress([FromRoute] AddressIdentifierDTO model)
        {
            var result = await _registerService.GetLoggedUserAddressAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
