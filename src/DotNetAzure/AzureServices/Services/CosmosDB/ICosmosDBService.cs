using MongoDB.Driver;

namespace AzureServices.Services.CosmosDB
{
    /// <summary>
    /// Defines a set of methods for managing database operations in a CosmosDB environment.
    /// This interface handles CRUD operations generically for a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the document stored in the database.</typeparam>
    public interface ICosmosDBService<T>
    {
        /// <summary>
        /// Adds an item to the database asynchronously.
        /// </summary>
        /// <param name="item">The item to add to the database.</param>
        /// <returns>A task that represents the asynchronous operation and contains the result of the operation.</returns>
        Task<CosmosDBResult<string?>> AddItemAsync(T? item);

        /// <summary>
        /// Updates an item in the database asynchronously using its identifier.
        /// </summary>
        /// <param name="id">The identifier of the item to update.</param>
        /// <param name="item">The new value of the item.</param>
        /// <returns>A task that represents the asynchronous operation and contains the result of the operation.</returns>
        Task<CosmosDBResult<string?>> UpdateItemAsync(Guid? id, T? item);

        /// <summary>
        /// Retrieves a single item from the database asynchronously using its identifier.
        /// </summary>
        /// <param name="id">The identifier of the item to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation and contains the result of the operation.</returns>
        Task<CosmosDBResult<T?>> GetItemAsync(Guid? id);

        /// <summary>
        /// Retrieves all items from the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation and contains the result of the operation.</returns>
        Task<CosmosDBResult<List<T>?>> GetItemsAsync();

        /// <summary>
        /// Retrieves items from the database asynchronously that match a specified filter.
        /// </summary>
        /// <param name="filter">The filter definition used to select items.</param>
        /// <returns>A task that represents the asynchronous operation and contains the result of the operation.</returns>
        Task<CosmosDBResult<List<T>?>> GetItemsAsync(FilterDefinition<T> filter);

        /// <summary>
        /// Deletes a single item from the database asynchronously using its identifier.
        /// </summary>
        /// <param name="id">The identifier of the item to delete.</param>
        /// <returns>A task that represents the asynchronous operation and contains the result of the operation.</returns>
        Task<CosmosDBResult<string?>> DeleteItemAsync(Guid? id);

        /// <summary>
        /// Deletes all items from the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation and contains the result of the operation.</returns>
        Task<CosmosDBResult<long?>> DeleteItemsAsync();

        /// <summary>
        /// Deletes items from the database asynchronously that match a specified filter.
        /// </summary>
        /// <param name="filter">The filter definition used to select items for deletion.</param>
        /// <returns>A task that represents the asynchronous operation and contains the result of the operation.</returns>
        Task<CosmosDBResult<long?>> DeleteItemsAsync(FilterDefinition<T> filter);
    }

}
