using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service class for managing report.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<ReportService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterService"/> class.
        /// </summary>
        /// <param name="uow">The unit of work for database operations.</param>
        /// <param name="logger">The logger.</param>
        public ReportService(IUnitOfWork uow, ILogger<ReportService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        /// <summary>
        /// Generates a report with user data in Excel format.
        /// </summary>
        /// <param name="filter">Filter criteria for selecting users.</param>
        /// <returns>Response containing the generated report as a byte array.</returns>
        public async Task <ResultResponse<byte[]>> GenerateUsersReport(UserFilterDTO filter)
        {
            var response = new ResultResponse<byte[]>();

            try
            {
                var users = await _uow.User.GetByFilterAsync(filter);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Users");

                    worksheet.Cell("A1").Style.Font.SetBold();
                    worksheet.Cell("B1").Style.Font.SetBold();
                    worksheet.Cell("C1").Style.Font.SetBold();

                    worksheet.Cell("A1").Value = "ID";
                    worksheet.Cell("B1").Value = "Name";
                    worksheet.Cell("C1").Value = "Email";

                    var currentRow = 2;

                    if (users.Any())
                    {
                        foreach (var item in users)
                        {
                            worksheet.Cell("A" + currentRow).Value = item.Id;
                            worksheet.Cell("B" + currentRow).Value = item.Name;
                            worksheet.Cell("C" + currentRow).Value = item.Email;

                            currentRow++;
                        }
                    }

                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        response.Data = ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating report");
                response.Message = "Unable to generate report.";
                response.Exception = ex;
            }

            return response;
        }
    }
}
