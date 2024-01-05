using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;

namespace CeciDapper.Infra.Data.Context
{
    /// <summary>
    /// Represents a database session responsible for creating and managing connections and transactions.
    /// </summary>
    public sealed class DbSession : IDisposable
    {
        /// <summary>
        /// Gets the active connection to the database.
        /// </summary>
        public IDbConnection Connection { get; }

        /// <summary>
        /// Gets or sets the active transaction associated with the database session.
        /// </summary>
        public IDbTransaction Transaction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbSession"/> class based on the provided configuration.
        /// </summary>
        /// <param name="configuration">The configuration containing the database connection information.</param>
        public DbSession(IConfiguration configuration)
        {
            // Creates a new connection using the connection string provided in the configuration.
            Connection = new MySqlConnection(configuration.GetConnectionString("CeciDatabase"));

            // Opens the connection to the database.
            Connection.Open();
        }

        /// <summary>
        /// Releases the resources used by the current instance of the <see cref="DbSession"/> class.
        /// </summary>
        public void Dispose() => Connection?.Dispose();
    }
}
