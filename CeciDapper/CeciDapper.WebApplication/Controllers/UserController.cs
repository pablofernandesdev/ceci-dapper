using CeciDapper.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Import;

namespace CeciDapper.WebApplication.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    //[Authorize(Policy = "Administrator")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IImportService _importService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="importService">The import service.</param>
        public UserController(IUserService userService,
            IImportService importService)
        {
            _userService = userService;
            _importService = importService;
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="model">The user details.</param>
        /// <returns>An action result indicating the success of the user creation.</returns>
        /// <response code="200">Returns success when the user is created.</response>
        /// <response code="400">Returns an error if the request fails.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Add([FromBody] UserAddDTO model)
        {
            var result = await _userService.AddAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="model">The updated user details.</param>
        /// <returns>An action result indicating the success of the user update.</returns>
        /// <response code="200">Returns success when the user is updated.</response>
        /// <response code="400">Returns an error if the request fails.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>  
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Update([FromBody] UserUpdateDTO model)
        {
            var result = await _userService.UpdateAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Updates the role of an existing user.
        /// </summary>
        /// <param name="model">The updated user role details.</param>
        /// <returns>An action result indicating the success of the user role update.</returns>
        /// <response code="200">Returns success when the user role is updated.</response>
        /// <response code="400">Returns an error if the request fails.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>    
        [HttpPut]
        [Route("update-role")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> UpdateRole([FromBody] UserUpdateRoleDTO model)
        {
            var result = await _userService.UpdateRoleAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="model">The user identifier.</param>
        /// <returns>An action result indicating the success of the user deletion.</returns>
        /// <response code="200">Returns success when the user is deleted.</response>
        /// <response code="400">Returns an error if the request fails.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>  
        [HttpDelete]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Delete([FromRoute] UserDeleteDTO model)
        {
            var result = await _userService.DeleteAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>An action result containing the list of users.</returns>
        /// <response code="200">Returns success with the list of users.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>   
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDataResponse<IEnumerable<UserResultDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultDataResponse<IEnumerable<UserResultDTO>>>> Get([FromQuery] UserFilterDTO filter)
        {
            var result = await _userService.GetAsync(filter);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves a user by their identifier.
        /// </summary>
        /// <param name="model">The user identifier.</param>
        /// <returns>An action result containing the user details.</returns>
        /// <response code="200">Returns success with the user details.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>  
        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<UserResultDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<UserResultDTO>>> GetById([FromRoute] UserIdentifierDTO model)
        {
            var result = await _userService.GetByIdAsync(model.UserId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Imports users.
        /// </summary>
        /// <param name="model">The file upload details.</param>
        /// <returns>An action result indicating the success of the user import.</returns>
        /// <response code="200">Returns success when the users are imported.</response>
        /// <response code="400">Returns an error if the request fails.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>  
        [HttpPost]
        [Route("import")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Import([FromForm] FileUploadDTO model)
        {
            var result = await _importService.ImportUsersAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
