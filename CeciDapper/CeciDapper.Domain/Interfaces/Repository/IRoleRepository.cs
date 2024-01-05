using CeciDapper.Domain.Entities;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Repository
{
    /// <summary>
    /// Represents the repository interface for performing CRUD operations on Role entities and additional methods related to roles.
    /// </summary>
    public interface IRoleRepository : IBaseRepository<Role>
    {
        /// <summary>
        /// Retrieves the basic profile role.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the basic profile role.</returns>
        Task<Role> GetBasicProfile();
    }
}
