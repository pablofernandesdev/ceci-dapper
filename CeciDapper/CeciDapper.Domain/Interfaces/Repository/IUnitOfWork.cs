using System;

namespace CeciDapper.Domain.Interfaces.Repository
{
    /// <summary>
    /// Represents a unit of work interface for managing repositories and coordinating transactions.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for managing User entities.
        /// </summary>
        IUserRepository User { get; }

        /// <summary>
        /// Gets the repository for managing Role entities.
        /// </summary>
        IRoleRepository Role { get; }

        /// <summary>
        /// Gets the repository for managing RefreshToken entities.
        /// </summary>
        IRefreshTokenRepository RefreshToken { get; }

        /// <summary>
        /// Gets the repository for managing RegistrationToken entities.
        /// </summary>
        IRegistrationTokenRepository RegistrationToken { get; }

        /// <summary>
        /// Gets the repository for managing ValidationCode entities.
        /// </summary>
        IValidationCodeRepository ValidationCode { get; }

        /// <summary>
        /// Gets the repository for managing Address entities.
        /// </summary>
        IAddressRepository Address { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
