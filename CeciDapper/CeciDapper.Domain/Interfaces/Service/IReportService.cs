using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.User;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for generating reports related to user information.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Generates a report containing user information based on the provided filtering criteria.
        /// </summary>
        /// <param name="filter">The DTO containing filtering criteria for generating the report.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse containing the generated report as a byte array.</returns>
        Task<ResultResponse<byte[]>> GenerateUsersReport(UserFilterDTO filter);
    }
}
