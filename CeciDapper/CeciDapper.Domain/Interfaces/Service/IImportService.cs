using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Import;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Service
{
    /// <summary>
    /// Represents a service interface for importing data, specifically users.
    /// </summary>
    public interface IImportService
    {
        /// <summary>
        /// Imports users asynchronously using the provided file upload data.
        /// </summary>
        /// <param name="model">The DTO containing file upload information.</param>
        /// <returns>A task representing the asynchronous operation. A ResultResponse indicating the outcome of the import operation.</returns>
        Task<ResultResponse> ImportUsersAsync(FileUploadDTO model);
    }
}
