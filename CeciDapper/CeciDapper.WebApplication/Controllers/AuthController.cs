using CeciDapper.Domain.DTO.Auth;
using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.ValidationCode;
using CeciDapper.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CeciDapper.WebApplication.Controllers
{
    /// <summary>
    /// Controller for user authentication and token management.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidationCodeService _validationCodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        /// <param name="validationCodeService">The validation code service.</param>
        public AuthController(IAuthService authService,
            IValidationCodeService validationCodeService)
        {
            _authService = authService;
            _validationCodeService = validationCodeService;
        }

        /// <summary>
        /// Authenticates a user and returns an authentication token.
        /// </summary>
        /// <param name="model">The login credentials of the user.</param>
        /// <returns>The result of the authentication as an action result.</returns>
        /// <response code="200">Returns success request autentication</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<AuthResultDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<AuthResultDTO>>> Auth([FromBody] LoginDTO model)
        {
            var result = await _authService.AuthenticateAsync(model, IpAddress());
            if (result.Data != null)
            {
                SetTokenCookie(result.Data.RefreshToken);
            }
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Refreshes the user's authentication token using the provided refresh token.
        /// </summary>
        /// <returns>The result of the token refresh operation as an action result.</returns>
        /// <response code="200">Returns success request autentication</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<AuthResultDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<AuthResultDTO>>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RefreshTokenAsync(refreshToken, IpAddress());
            if (result.Data != null)
            {
                SetTokenCookie(result.Data.RefreshToken);
            }
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Revokes the user's refresh token, effectively logging the user out.
        /// </summary>
        /// <returns>The result of the token revocation operation as an action result.</returns>
        /// <response code="200">Returns success revoking auth token</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("revoke-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> RevokeToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RevokeTokenAsync(refreshToken, IpAddress());
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Request user password recovery
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when requesting password recovery</returns>
        /// <response code="200">Returns when requesting password recovery</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            var result = await _authService.ForgotPasswordAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Sends a validation code to the authenticated user.
        /// </summary>
        /// <returns>The result of the validation code sending operation as an action result.</returns>
        /// <response code="200">Returns when requesting validation code</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [Authorize]
        [HttpPost]
        [Route("send-validation-code")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> SendValidationCode()
        {
            var result = await _validationCodeService.SendAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Validates a validation code entered by the authenticated user.
        /// </summary>
        /// <param name="model">The validation code to validate.</param>
        /// <returns>The result of the validation code validation operation as an action result.</returns>
        /// <response code="200">Returns when validating validation code</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [Authorize]
        [HttpPost]
        [Route("validate-validation-code")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> ValidateValidationCode([FromBody] ValidationCodeValidateDTO model)
        {
            var result = await _validationCodeService.ValidateCodeAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        // helper methods

        /// <summary>
        /// Sets the authentication token as a cookie in the response.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        /// <summary>
        /// Retrieves the client's IP address from the request headers.
        /// </summary>
        /// <returns>The client's IP address.</returns>
        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
