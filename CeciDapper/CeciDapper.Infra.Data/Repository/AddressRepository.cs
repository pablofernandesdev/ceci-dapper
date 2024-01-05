using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Infra.Data.Context;
using Dapper;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace CeciDapper.Infra.Data.Repository
{
    /// <summary>
    /// Represents a repository for managing addresses in the database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressRepository"/> class.
        /// </summary>
        /// <param name="session">The database session.</param>
        public AddressRepository(DbSession session) : base(session)
        {
        }

        /// <summary>
        /// Retrieves a list of addresses based on the provided filter asynchronously.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>A collection of addresses that match the filter.</returns>
        public async Task<IEnumerable<Address>> GetByFilterAsync(AddressFilterDTO filter)
        {
            StringBuilder query = new StringBuilder();

            query.Append($"SELECT * FROM {GetTableName()} A" +
                $" INNER JOIN User U on A.{nameof(Address.UserId)} = U.{nameof(User.Id)}" +
                $" WHERE A.{nameof(Address.Active)} = 1");

            if (!string.IsNullOrEmpty(filter.District))
            {
                query.Append($" AND A.{nameof(Address.District)} LIKE CONCAT('%',@{nameof(Address.District)},'%')");
            }

            if (!string.IsNullOrEmpty(filter.Locality))
            {
                query.Append($" AND A.{nameof(Address.Locality)} LIKE CONCAT('%',@{nameof(Address.Locality)},'%')");
            }

            if (!string.IsNullOrEmpty(filter.Uf))
            {
                query.Append($" AND A.{nameof(Address.Uf)} = @{nameof(Address.Uf)}");
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query.Append($" AND (A.{nameof(Address.District)} = @{nameof(Address.District)} OR A.{nameof(Address.Locality)} = @{nameof(Address.Locality)})");
            }

            return await _session.Connection.QueryAsync<Address, User, Address>(sql: query.ToString(),
                param: filter,
                transaction: _session.Transaction,
                map: (address, user) =>
                {
                    address.User = user;
                    return address;
                },
                splitOn: nameof(Address.UserId));
        }

        /// <summary>
        /// Retrieves a list of addresses for the logged-in user based on the provided filter asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the logged-in user.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>A collection of addresses that match the filter for the logged-in user.</returns>
        public async Task<IEnumerable<Address>> GetLoggedUserAddressesAsync(int userId, AddressFilterDTO filter)
        {
            StringBuilder query = new StringBuilder();

            query.Append($"SELECT * FROM {GetTableName()} A" +
                $" INNER JOIN User U on A.{nameof(Address.UserId)} = U.{nameof(User.Id)}" +
                $" WHERE A.{nameof(Address.UserId)} = {userId}");

            if (!string.IsNullOrEmpty(filter.District))
            {
                query.Append($" AND A.{nameof(Address.District)} LIKE CONCAT('%',@{nameof(Address.District)},'%')");
            }

            if (!string.IsNullOrEmpty(filter.Locality))
            {
                query.Append($" AND A.{nameof(Address.Locality)} LIKE CONCAT('%',@{nameof(Address.Locality)},'%')");
            }

            if (!string.IsNullOrEmpty(filter.Uf))
            {
                query.Append($" AND A.{nameof(Address.Uf)} = @{nameof(Address.Uf)}");
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query.Append($" AND (A.{nameof(Address.District)} = @{nameof(Address.District)} OR A.{nameof(Address.Locality)} = @{nameof(Address.Locality)})");
            }

            return await _session.Connection.QueryAsync<Address, User, Address>(sql: query.ToString(),
                param: filter,
                transaction: _session.Transaction,
                map: (address, user) =>
                {
                    address.User = user;
                    return address;
                },
                splitOn: nameof(Address.UserId));
        }

        /// <summary>
        /// Retrieves the total count of addresses based on the provided filter asynchronously.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>The total count of addresses that match the filter.</returns>
        public async Task<int> GetTotalByFilterAsync(AddressFilterDTO filter)
        {
            StringBuilder query = new StringBuilder();

            query.Append($"SELECT COUNT(A.{nameof(Address.Id)}) FROM {GetTableName()} A" +
                $" INNER JOIN User U on A.{nameof(Address.UserId)} = U.{nameof(User.Id)}" +
                $" WHERE A.{nameof(Address.Active)} = 1");

            if (!string.IsNullOrEmpty(filter.District))
            {
                query.Append($" AND A.{nameof(Address.District)} LIKE CONCAT('%',@{nameof(Address.District)},'%')");
            }

            if (!string.IsNullOrEmpty(filter.Locality))
            {
                query.Append($" AND A.{nameof(Address.Locality)} LIKE CONCAT('%',@{nameof(Address.Locality)},'%')");
            }

            if (!string.IsNullOrEmpty(filter.Uf))
            {
                query.Append($" AND A.{nameof(Address.Uf)} = @{nameof(Address.Uf)}");
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query.Append($" AND (A.{nameof(Address.District)} = @{nameof(Address.District)} OR A.{nameof(Address.Locality)} = @{nameof(Address.Locality)})");
            }

            return await _session.Connection.QueryFirstAsync<int>(query.ToString(), filter, transaction: _session.Transaction);
        }

        /// <summary>
        /// Retrieves the total count of addresses for the logged-in user based on the provided filter asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the logged-in user.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>The total count of addresses that match the filter for the logged-in user.</returns>
        public async Task<int> GetTotalLoggedUserAddressesAsync(int userId, AddressFilterDTO filter)
        {
            StringBuilder query = new StringBuilder();

            query.Append($"SELECT COUNT(A.{nameof(Address.Id)}) FROM {GetTableName()} A" +
                $" INNER JOIN User U on A.{nameof(Address.UserId)} = U.{nameof(User.Id)}" +
                $" WHERE A.{nameof(Address.UserId)} = {userId}");

            if (!string.IsNullOrEmpty(filter.District))
            {
                query.Append($" AND A.{nameof(Address.District)} LIKE CONCAT('%',@{nameof(Address.District)},'%')");
            }

            if (!string.IsNullOrEmpty(filter.Locality))
            {
                query.Append($" AND A.{nameof(Address.Locality)} LIKE CONCAT('%',@{nameof(Address.Locality)},'%')");
            }

            if (!string.IsNullOrEmpty(filter.Uf))
            {
                query.Append($" AND A.{nameof(Address.Uf)} = @{nameof(Address.Uf)}");
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query.Append($" AND (A.{nameof(Address.District)} = @{nameof(Address.District)} OR A.{nameof(Address.Locality)} = @{nameof(Address.Locality)})");
            }

            return await _session.Connection.QueryFirstAsync<int>(query.ToString(), filter, transaction: _session.Transaction);
        }
    }
}
