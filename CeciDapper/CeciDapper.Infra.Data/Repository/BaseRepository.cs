using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Infra.CrossCutting.QueryAttributes;
using CeciDapper.Infra.Data.Context;
using Dapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CeciDapper.Infra.Data.Repository
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Base repository class for CRUD operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbSession _session;
        private readonly string _tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="session">The database session.</param>
        public BaseRepository(DbSession session)
        {
            _session = session;
            _tableName = GetTableName();
        }

        /// <summary>
        /// Asynchronously adds a new entity to the database.
        /// </summary>
        /// <param name="obj">The entity to add.</param>
        /// <returns>The added entity if the operation is successful, otherwise null.</returns>
        public async Task<TEntity> AddAsync(TEntity obj)
        {
            var query = GenerateInsertQuery();

            return await _session.Connection.ExecuteAsync(query, obj, transaction: _session.Transaction) > 0 ? obj : null;
        }

        /// <summary>
        /// Asynchronously adds a range of entities to the database.
        /// </summary>
        /// <param name="obj">The range of entities to add.</param>
        /// <returns>The added entities if the operation is successful, otherwise null.</returns>
        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> obj)
        {
            var query = GenerateInsertQuery();

            return await _session.Connection.ExecuteAsync(query, obj, transaction: _session.Transaction) > 0 ? obj : null;
        }

        /// <summary>
        /// Asynchronously updates an entity in the database.
        /// </summary>
        /// <param name="obj">The entity to update.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        public async Task<bool> UpdateAsync(TEntity obj)
        {
            var query = GenerateUpdateQuery();

            return await _session.Connection.ExecuteAsync(query, obj, transaction: _session.Transaction) > 0;
        }

        /// <summary>
        /// Asynchronously deletes an entity from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _session.Connection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = id }, transaction: _session.Transaction) > 0;
        }

        /// <summary>
        /// Asynchronously retrieves all entities from the database.
        /// </summary>
        /// <returns>A collection of entities.</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _session.Connection.QueryAsync<TEntity>($"SELECT * FROM {_tableName}", transaction: _session.Transaction);
        }

        /// <summary>
        /// Asynchronously retrieves an entity from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The retrieved entity if found, otherwise throws a KeyNotFoundException.</returns>
        public async Task<TEntity> GetByIdAsync(int id)
        {
            var result = await _session.Connection.QuerySingleOrDefaultAsync<TEntity>($"SELECT * FROM {_tableName} WHERE Id=@Id", new { Id = id }, transaction: _session.Transaction);
            if (result == null)
                throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");

            return result;
        }

        /// <summary>
        /// Asynchronously retrieves entities from the database based on a query condition.
        /// </summary>
        /// <param name="queryCondition">The condition to filter the entities.</param>
        /// <returns>A collection of entities that match the condition.</returns>
        public async Task<IEnumerable<TEntity>> GetAsync(string queryCondition)
        {
            return await _session.Connection.QueryAsync<TEntity>($"SELECT * FROM {_tableName} WHERE {queryCondition}", transaction: _session.Transaction);
        }

        /// <summary>
        /// Asynchronously retrieves the first entity from the database that matches a query condition.
        /// </summary>
        /// <param name="queryCondition">The condition to filter the entity.</param>
        /// <returns>The first entity that matches the condition, or null if no entity is found.</returns>
        public async Task<TEntity> GetFirstOrDefaultAsync(string queryCondition)
        {
            return await _session.Connection.QueryFirstOrDefaultAsync<TEntity>($"SELECT * FROM {_tableName} WHERE {queryCondition}", transaction: _session.Transaction);
        }

        /// <summary>
        /// Asynchronously retrieves the total number of entities that match a query condition.
        /// </summary>
        /// <param name="queryCondition">The condition to filter the entities.</param>
        /// <returns>The total number of entities that match the condition.</returns>
        public async Task<int> GetTotalAsync(string queryCondition)
        {
            return await _session.Connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) {_tableName} WHERE {queryCondition}", transaction: _session.Transaction);
        }

        /// <summary>
        /// Generates an SQL INSERT query for the current table and entity.
        /// </summary>
        /// <returns>An SQL INSERT query string.</returns>
        private string GenerateInsertQuery()
        {
            string columns = GetColumns(excludeKey: true);
            string properties = GetPropertyNames(excludeKey: true);
            return $"INSERT INTO {_tableName} ({columns}) VALUES ({properties})";
        }

        /// <summary>
        /// Generates an SQL UPDATE query for the current table and entity.
        /// </summary>
        /// <returns>An SQL UPDATE query string.</returns>
        private string GenerateUpdateQuery()
        {
            string keyColumn = GetKeyColumnName();
            string keyProperty = GetKeyPropertyName();

            StringBuilder query = new StringBuilder();
            query.Append($"UPDATE {_tableName} SET ");

            foreach (var property in GetProperties(true))
            {
                var columnAttr = property.GetCustomAttribute<ColumnAttribute>();

                string propertyName = property.Name;
                string columnName = columnAttr.Name;

                query.Append($"{columnName} = @{propertyName},");
            }

            query.Remove(query.Length - 1, 1);

            query.Append($" WHERE {keyColumn} = @{keyProperty}");

            return query.ToString();
        }

        /// <summary>
        /// Gets the name of the database table associated with the entity.
        /// </summary>
        /// <returns>The name of the database table.</returns>
        protected string GetTableName()
        {
            string tableName = "";
            var type = typeof(TEntity);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name;
        }

        /// <summary>
        /// Gets the name of the primary key column associated with the entity.
        /// </summary>
        /// <returns>The name of the primary key column.</returns>
        public static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the names of columns in the database table associated with the entity.
        /// </summary>
        /// <param name="excludeKey">True to exclude the primary key column, otherwise false.</param>
        /// <returns>A comma-separated string of column names.</returns>
        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(TEntity);
            var columns = string.Join(", ", type.GetProperties().Where(pi => pi.GetCustomAttributes(typeof(IgnoreQueryAttribute), true).Length == 0).ToArray()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        /// <summary>
        /// Gets the names of properties in the entity class.
        /// </summary>
        /// <param name="excludeKey">True to exclude the primary key property, otherwise false.</param>
        /// <returns>A comma-separated string of property names.</returns>
        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(TEntity).GetProperties().Where(pi => pi.GetCustomAttributes(typeof(IgnoreQueryAttribute), true).Length == 0).ToArray()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }

        /// <summary>
        /// Gets a collection of property information for the entity class.
        /// </summary>
        /// <param name="excludeKey">True to exclude the primary key property, otherwise false.</param>
        /// <returns>A collection of property information.</returns>
        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(TEntity).GetProperties().Where(pi => pi.GetCustomAttributes(typeof(IgnoreQueryAttribute), true).Length == 0).ToArray()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        /// <summary>
        /// Gets the name of the primary key property in the entity class.
        /// </summary>
        /// <returns>The name of the primary key property.</returns>
        protected string GetKeyPropertyName()
        {
            var properties = typeof(TEntity).GetProperties().Where(pi => pi.GetCustomAttributes(typeof(IgnoreQueryAttribute), true).Length == 0).ToArray()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }
    }
}
