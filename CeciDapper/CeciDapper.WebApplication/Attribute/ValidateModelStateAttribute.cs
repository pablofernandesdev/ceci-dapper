using CeciDapper.Domain.DTO.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CeciDapper.WebApplication.Attribute
{
    /// <summary>
    /// Attribute used to validate the state of the model before executing an action in an ASP.NET Core API.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Method executed before an action is executed.
        /// Checks if the model state is valid and, if not, returns an error response with details of the invalid model.
        /// </summary>
        /// <param name="context">The execution context of the action.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // Get the error messages from the ModelState
                var errors = string.Join('\n', context.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage));

                // Create an error response object
                var responseObj = new ResultResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = errors
                };

                // Set the action result as a JSON error response with status code 400 (BadRequest)
                context.Result = new JsonResult(responseObj)
                {
                    StatusCode = (int)System.Net.HttpStatusCode.BadRequest
                };
            }
        }
    }
}
