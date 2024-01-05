using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace CeciDapper.Infra.Data.Repository
{
    /// <summary>
    /// Repository for managing ValidationCode entities.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ValidationCodeRepository : BaseRepository<ValidationCode>, IValidationCodeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationCodeRepository"/> class.
        /// </summary>
        /// <param name="session">The database session.</param>
        public ValidationCodeRepository(DbSession session) : base(session)
        {
        }
    }
}
