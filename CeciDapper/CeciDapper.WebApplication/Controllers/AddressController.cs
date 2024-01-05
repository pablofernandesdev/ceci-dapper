using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciDapper.WebApplication.Controllers
{
    /// <summary>
    /// Controller class for managing address-related operations in the API.
    /// </summary>
    [Route("api/address")]
    [ApiController]
    [Authorize(Policy = "Administrator")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressController"/> class.
        /// </summary>
        /// <param name="addressService">The address service used for address-related operations.</param>
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        /// <summary>
        /// Retrieves an address by its ZIP code.
        /// </summary>
        /// <param name="model">The ZIP code of the address to retrieve.</param>
        /// <returns>The result of the operation as an action result.</returns>
        /// <response code="200">Returns success when get address</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>   
        //[Authorize(Policy = "Basic")]
        [HttpGet]
        [Route("zip-code/{zipCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<AddressResultDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<AddressResultDTO>>> GetByZipCode([FromRoute] AddressZipCodeDTO model)
        {
            var result = await _addressService.GetAddressByZipCodeAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Adds a new address.
        /// </summary>
        /// <param name="model">The address to add.</param>
        /// <returns>The result of the operation as an action result.</returns>
        /// <response code="200">Returns success when create address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Add([FromBody] AddressAddDTO model)
        {
            var result = await _addressService.AddAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="model">The address to update.</param>
        /// <returns>The result of the operation as an action result.</returns>
        /// <response code="200">Returns success when update address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Update([FromBody] AddressUpdateDTO model)
        {
            var result = await _addressService.UpdateAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Deletes an address.
        /// </summary>
        /// <param name="model">The identifier of the address to delete.</param>
        /// <returns>The result of the operation as an action result.</returns>
        /// <response code="200">Returns success when deleted address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpDelete]
        [Route("{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]

        public async Task<ActionResult<ResultResponse>> Delete([FromRoute] AddressDeleteDTO model)
        {
            var result = await _addressService.DeleteAsync(model.AddressId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves a list of addresses based on filtering criteria.
        /// </summary>
        /// <param name="model">The filtering criteria for the addresses.</param>
        /// <returns>The result of the operation as an action result.</returns>
        /// <response code="200">Returns success when get all adresses</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>  
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDataResponse<IEnumerable<AddressResultDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultDataResponse<IEnumerable<ResultDataResponse<AddressResultDTO>>>>> Get([FromQuery] AddressFilterDTO model)
        {
            var result = await _addressService.GetAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves an address by its identifier.
        /// </summary>
        /// <param name="model">The identifier of the address to retrieve.</param>
        /// <returns>The result of the operation as an action result.</returns>
        /// <response code="200">Returns success when get address by id</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [Route("{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<AddressResultDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<AddressResultDTO>>> GetById([FromRoute] AddressIdentifierDTO model)
        {
            var result = await _addressService.GetByIdAsync(model.AddressId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
