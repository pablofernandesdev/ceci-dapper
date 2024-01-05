using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Infra.CrossCutting.Settings;
using CeciDapper.Infra.Data.Context;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace CeciDapper.Infra.Data.Repository
{
    /// <summary>
    /// Repository for managing Role entities.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly RoleSettings _roleSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="session">The database session.</param>
        /// <param name="roleSettings">The profile settings containing profile-related information.</param>
        public RoleRepository(DbSession session, IOptions<RoleSettings> roleSettings) : base(session)
        {
            _roleSettings = roleSettings.Value;
        }

        /// <summary>
        /// Retrieves the basic profile role.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains the basic profile role.</returns>
        public async Task<Role> GetBasicProfile()
        {
            return await GetFirstOrDefaultAsync($"{nameof(Role.Name)} = '{_roleSettings.BasicRoleName}'");
        }
    }
}
