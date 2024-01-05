using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Role;
using CeciDapper.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciDapper.WebApplication.Controllers
{
    /// <summary>
    /// Controller for managing roles.
    /// </summary>
    [Route("api/role")]
    [ApiController]
    [Authorize(Policy = "Administrator")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="roleService">The role service.</param>
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Adds a new role.
        /// </summary>
        /// <param name="model">The role details.</param>
        /// <returns>An action result indicating the success of the role creation.</returns>
        /// <response code="200">Returns success when the role is created.</response>
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
        public async Task<ActionResult<ResultResponse>> Add([FromBody] RoleAddDTO model)
        {
            var result = await _roleService.AddAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="model">The updated role details.</param>
        /// <returns>An action result indicating the success of the role update.</returns>
        /// <response code="200">Returns success when the role is updated.</response>
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
        public async Task<ActionResult<ResultResponse>> Update([FromBody] RoleUpdateDTO model)
        {
            var result = await _roleService.UpdateAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="model">The role identifier.</param>
        /// <returns>An action result indicating the success of the role deletion.</returns>
        /// <response code="200">Returns success when the role is deleted.</response>
        /// <response code="400">Returns an error if the request fails.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>   
        [HttpDelete]
        [Route("{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]

        public async Task<ActionResult<ResultResponse>> Delete([FromRoute] RoleDeleteDTO model)
        {
            var result = await _roleService.DeleteAsync(model.RoleId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves all roles.
        /// </summary>
        /// <returns>An action result containing the list of roles.</returns>
        /// <response code="200">Returns success with the list of roles.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>   
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDataResponse<IEnumerable<RoleResultDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultDataResponse<IEnumerable<ResultDataResponse<RoleResultDTO>>>>> Get()
        {
            var result = await _roleService.GetAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves a role by its identifier.
        /// </summary>
        /// <param name="model">The role identifier.</param>
        /// <returns>An action result containing the role details.</returns>
        /// <response code="200">Returns success with the role details.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>   
        [HttpGet]
        [Route("{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<RoleResultDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<RoleResultDTO>>> GetById([FromRoute] IdentifierRoleDTO model)
        {
            var result = await _roleService.GetByIdAsync(model.RoleId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
