using CeciDapper.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciDapper.Domain.Interfaces.Repository
{
    /// <summary>
    /// Represents the base repository interface for CRUD operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity that the repository works with.</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="obj">The entity to be added.</param>
        /// <returns>The added entity.</returns>
        Task<TEntity> AddAsync(TEntity obj);

        /// <summary>
        /// Adds a range of entities asynchronously.
        /// </summary>
        /// <param name="obj">The collection of entities to be added.</param>
        /// <returns>The added collection of entities.</returns>
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> obj);

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="obj">The entity to be updated.</param>
        /// <returns>True if the entity was successfully updated, otherwise false.</returns>
        Task<bool> UpdateAsync(TEntity obj);

        /// <summary>
        /// Deletes an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the entity to be deleted.</param>
        /// <returns>True if the entity was successfully deleted, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Retrieves entities based on the specified query condition asynchronously.
        /// </summary>
        /// <param name="queryCondition">The query condition to filter entities.</param>
        /// <returns>A collection of entities that match the query condition.</returns>
        Task<IEnumerable<TEntity>> GetAsync(string queryCondition);

        /// <summary>
        /// Retrieves the first entity that matches the specified query condition asynchronously.
        /// </summary>
        /// <param name="queryCondition">The query condition to filter entities.</param>
        /// <returns>The first entity that matches the query condition, or null if not found.</returns>
        Task<TEntity> GetFirstOrDefaultAsync(string queryCondition);

        /// <summary>
        /// Retrieves the total count of entities that match the specified query condition asynchronously.
        /// </summary>
        /// <param name="queryCondition">The query condition to filter entities.</param>
        /// <returns>The total count of entities that match the query condition.</returns>
        Task<int> GetTotalAsync(string queryCondition);
    }
}
