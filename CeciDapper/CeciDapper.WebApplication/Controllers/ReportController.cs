using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace CeciDapper.WebApplication.Controllers
{
    /// <summary>
    /// Controller for generating reports.
    /// </summary>
    [Route("api/report")]
    [ApiController]
    [Authorize(Policy = "Administrator")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportController"/> class.
        /// </summary>
        /// <param name="reportService">The report service.</param>
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Generates a report of users based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter parameters for the report.</param>
        /// <returns>An action result containing the users report file.</returns>
        /// <response code="200">Returns success when the users report is generated.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="500">Internal server error.</response>   
        [HttpGet]
        [Route("excel/users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<IActionResult> GenerateUsersReport([FromQuery] UserFilterDTO filter)
        {
            var result = await _reportService.GenerateUsersReport(filter);
            if (result.Data != null)
            {
                return File(new MemoryStream(result.Data), "application/octet-stream", "users_report.xlsx");
            }

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
