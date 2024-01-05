using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace CeciDapper.Infra.Data.Repository
{
    /// <summary>
    /// Repository for managing RefreshToken entities.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenRepository"/> class.
        /// </summary>
        /// <param name="session">The database session.</param>
        public RefreshTokenRepository(DbSession session) : base(session)
        {

        }
    }
}
