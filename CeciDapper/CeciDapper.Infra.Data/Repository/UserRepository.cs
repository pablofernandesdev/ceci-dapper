using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Infra.Data.Context;
using Dapper;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace CeciDapper.Infra.Data.Repository
{
    /// <summary>
    /// Class that implements the IUserRepository interface and provides methods to access user data in the database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="session">The database session.</param>
        public UserRepository(DbSession session) : base(session)
        {
        }

        /// <summary>
        /// Retrieves a list of users based on the provided filters.
        /// </summary>
        /// <param name="filter">A UserFilterDTO object containing filter criteria.</param>
        /// <returns>A collection of User objects that match the filter criteria.</returns>
        public async Task<IEnumerable<User>> GetByFilterAsync(UserFilterDTO filter)
        {
            StringBuilder query = new StringBuilder();

            query.Append($"SELECT * FROM {GetTableName()} U" +
                $" INNER JOIN Role R on U.{nameof(User.RoleId)} = R.{nameof(Role.Id)}" +
                $" WHERE U.{nameof(User.Active)} = 1");

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query.Append($" AND U.{nameof(User.Name)} LIKE CONCAT('%',@{nameof(User.Name)},'%')");
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query.Append($" AND U.{nameof(User.Email)} = @{nameof(User.Email)}");
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query.Append($" AND (U.{nameof(User.Name)} = @{nameof(User.Name)} OR U.{nameof(User.Email)} = @{nameof(User.Email)})");
            }

            return await _session.Connection.QueryAsync<User, Role, User>(sql: query.ToString(), 
                param: filter, 
                transaction: _session.Transaction,
                map: (user, role) =>
                {
                    user.Role = role;
                    return user;
                },
                splitOn: nameof(User.RoleId));
        }

        /// <summary>
        /// Retrieves the total number of users based on the provided filters.
        /// </summary>
        /// <param name="filter">A UserFilterDTO object containing filter criteria.</param>
        /// <returns>The total number of users that match the filter criteria.</returns>
        public async Task<int> GetTotalByFilterAsync(UserFilterDTO filter)
        {
            StringBuilder query = new StringBuilder();

            query.Append($"SELECT COUNT(U.{nameof(User.Id)}) FROM {GetTableName()} U" +
                $" INNER JOIN Role R on U.{nameof(User.RoleId)} = R.{nameof(Role.Id)}" +
                $" WHERE U.{nameof(User.Active)} = 1");

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query.Append($" AND U.{nameof(User.Name)} LIKE CONCAT('%',@{nameof(User.Name)},'%')");
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query.Append($" AND U.{nameof(User.Email)} = @{nameof(User.Email)}");
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query.Append($" AND (U.{nameof(User.Name)} = @{nameof(User.Name)} OR U.{nameof(User.Email)} = @{nameof(User.Email)})");
            }

            return await _session.Connection.QueryFirstAsync<int>(query.ToString(), filter, transaction: _session.Transaction);
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The User object corresponding to the specified ID, including information about the user's role.</returns>
        public async Task<User> GetUserByIdAsync(int id)
        {
            StringBuilder query = new StringBuilder();

            query.Append($"SELECT * FROM {GetTableName()} U" +
                $" INNER JOIN Role R on U.{nameof(User.RoleId)} = R.{nameof(Role.Id)}" +
                $" WHERE U.{nameof(User.Id)} = @{nameof(User.Id)}");

            var user = await _session.Connection.QueryAsync<User, Role, User>(sql: query.ToString(),
                param: new { Id = id },
                transaction: _session.Transaction,
                map: (user, role) =>
                {
                    user.Role = role;
                    return user;
                },
                splitOn: nameof(User.RoleId));

            return user.FirstOrDefault();
        }
    }
}
