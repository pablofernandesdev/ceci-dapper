using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace CeciDapper.Infra.Data.Repository
{
    /// <summary>
    /// Unit of Work implementation for managing repositories.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Gets the repository for managing User entities.
        /// </summary>
        public IUserRepository User { get; }

        /// <summary>
        /// Gets the repository for managing Role entities.
        /// </summary>
        public IRoleRepository Role { get; }

        /// <summary>
        /// Gets the repository for managing RefreshToken entities.
        /// </summary>
        public IRefreshTokenRepository RefreshToken { get; }

        /// <summary>
        /// Gets the repository for managing RegistrationToken entities.
        /// </summary>
        public IRegistrationTokenRepository RegistrationToken { get; }

        /// <summary>
        /// Gets the repository for managing ValidationCode entities.
        /// </summary>
        public IValidationCodeRepository ValidationCode { get; }

        /// <summary>
        /// Gets the repository for managing Address entities.
        /// </summary>
        public IAddressRepository Address { get; }

        private readonly DbSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="session">The database session.</param>
        /// <param name="userRepository">The repository for managing User entities.</param>
        /// <param name="roleRepository">The repository for managing Role entities.</param>
        /// <param name="refreshTokenRepository">The repository for managing RefreshToken entities.</param>
        /// <param name="registrationToken">The repository for managing RegistrationToken entities.</param>
        /// <param name="validationCode">The repository for managing ValidationCode entities.</param>
        /// <param name="address">The repository for managing Address entities.</param>
        public UnitOfWork(
            DbSession session,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IRegistrationTokenRepository registrationToken,
            IValidationCodeRepository validationCode,
            IAddressRepository address)
        {
            _session = session;
            User = userRepository;
            Role = roleRepository;
            RefreshToken = refreshTokenRepository;
            RegistrationToken = registrationToken;
            ValidationCode = validationCode;
            Address = address;
        }

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        public void BeginTransaction()
        {
            _session.Transaction = _session.Connection.BeginTransaction();
        }

        /// <summary>
        /// Commits the transaction to save changes to the database.
        /// </summary>
        public void Commit()
        {
            _session.Transaction.Commit();
            Dispose();
        }

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        public void Rollback()
        {
            _session.Transaction.Rollback();
            Dispose();
        }

        /// <summary>
        /// Disposes of the Unit of Work and releases any allocated resources.
        /// </summary>
        public void Dispose() => _session.Transaction?.Dispose();
    }
}
