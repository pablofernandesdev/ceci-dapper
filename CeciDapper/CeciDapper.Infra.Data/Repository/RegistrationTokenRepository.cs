using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace CeciDapper.Infra.Data.Repository
{
    /// <summary>
    /// Repository for managing RegistrationToken entities.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RegistrationTokenRepository : BaseRepository<RegistrationToken>, IRegistrationTokenRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationTokenRepository"/> class.
        /// </summary>
        /// <param name="session">The database session.</param>
        public RegistrationTokenRepository(DbSession session) : base(session)
        {
        }
    }
}
