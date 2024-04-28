
using MongoDB.Bson;
using MongoDB.Driver;

namespace AzureServices.Services.CosmosDB
{
    public class CosmosDBService<T> : ICosmosDBService<T> where T : class
    {
        // Nuget: MongoDB.Driver 2.25.0
        private readonly IMongoCollection<T> _collection;

        public CosmosDBService(string connectionString, string databaseName, string collection)
        {
            if(string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"'{nameof(connectionString)}' cannot be null or empty", nameof(connectionString));
            }

            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentException($"'{nameof(databaseName)}' cannot be null or empty", nameof(databaseName));
            }

            if (string.IsNullOrEmpty(collection))
            {
                throw new ArgumentException($"'{nameof(collection)}' cannot be null or empty", nameof(collection));
            }

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<T>(collection);
        }

        public async Task<CosmosDBResult<string?>> AddItemAsync(T? item)
        {
            try
            {
                //check if null
                if (item == null)
                {
                    return new CosmosDBErrorResult<string?>(new List<string> { $"{nameof(item)} is null" }, CosmosDBResultCode.BadRequest);
                }

                //insert the item, if it fails it should go to 
                await _collection.InsertOneAsync(item);
                return new CosmosDBEmptySuccessResult<string?>(new List<string> { });

            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new CosmosDBErrorResult<string?>(new List<string> { "Unexpected problem occured.", ex.Message }, CosmosDBResultCode.Error);
            }
        }

        public async Task<CosmosDBResult<long?>> DeleteItemsAsync()
        {
            try
            {
                var result = await _collection.DeleteManyAsync(new BsonDocument());

                //return bad request if the result was not acknowledged
                if (!result.IsAcknowledged)
                {
                    return new CosmosDBErrorResult<long?>(new List<string> { "The result was not Acknowledged" }, CosmosDBResultCode.Error);

                }
                //return bad request if no items were found
                if (result.DeletedCount == 0)
                {
                    return new CosmosDBErrorResult<long?>(new List<string> { "No items found to delete" }, CosmosDBResultCode.BadRequest);
                }

                return new CosmosDBSuccessResult<long?>(new List<string> { }, result.DeletedCount);
            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new CosmosDBErrorResult<long?>(new List<string> { "Unexpected problem occured.", ex.Message }, CosmosDBResultCode.Error);
            }
        }

        public async Task<CosmosDBResult<long?>> DeleteItemsAsync(FilterDefinition<T> filter)
        {
            try
            {
                var result = await _collection.DeleteManyAsync(filter);

                //return bad request if the result was not acknowledged
                if (!result.IsAcknowledged)
                {
                    return new CosmosDBErrorResult<long?>(new List<string> { "The result was not Acknowledged" }, CosmosDBResultCode.Error);

                }
                //return bad request if no items were found
                if (result.DeletedCount == 0)
                {
                    return new CosmosDBErrorResult<long?>(new List<string> { "No items found to delete" }, CosmosDBResultCode.BadRequest);
                }

                return new CosmosDBSuccessResult<long?>(new List<string> { }, result.DeletedCount);
            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new CosmosDBErrorResult<long?>(new List<string> { "Unexpected problem occured.", ex.Message }, CosmosDBResultCode.Error);
            }
        }

        public async Task<CosmosDBResult<string?>> DeleteItemAsync(Guid? id)
        {
            try
            {
                //check if paramter is null
                if (id == null)
                {
                    return new CosmosDBErrorResult<string?>(new List<string> { $"{nameof(id)} is null" }, CosmosDBResultCode.BadRequest);
                }

                var result = await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));

                //return bad request if the result was not acknowledged
                if (!result.IsAcknowledged)
                {
                    return new CosmosDBErrorResult<string?>(new List<string> { "The result was not Acknowledged" }, CosmosDBResultCode.Error);
                }
                
                //return bad request if no items were found
                if (result.DeletedCount == 0)
                {
                    return new CosmosDBErrorResult<string?>(new List<string> { "No items found to delete" }, CosmosDBResultCode.BadRequest);
                }

                //return bad request if more than one item was deleted
                if (result.DeletedCount > 1)
                {
                    return new CosmosDBErrorResult<string?>(new List<string> { "More than one item was deleted", $"{result.DeletedCount} items was removed." }, CosmosDBResultCode.BadRequest);
                }

                return new CosmosDBEmptySuccessResult<string?>(new List<string> { });
            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new CosmosDBErrorResult<string?>(new List<string> { "Unexpected problem occured.", ex.Message }, CosmosDBResultCode.Error);
            }
        }

        public async Task<CosmosDBResult<List<T>?>> GetItemsAsync()
        {
            try
            {
                var result = await _collection.Find(new BsonDocument()).ToListAsync();

                //return bad request if no items were found
                if (result.Count == 0)
                {
                    return new CosmosDBErrorResult<List<T>?>(new List<string> { "No items found" }, CosmosDBResultCode.BadRequest);
                }

                return new CosmosDBSuccessResult<List<T>?>(new List<string> { }, result);

            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new CosmosDBErrorResult<List<T>?>(new List<string> { "Unexpected problem occured.", ex.Message }, CosmosDBResultCode.Error);
            }
        }
        
        public async Task<CosmosDBResult<List<T>?>> GetItemsAsync(FilterDefinition<T> filter)
        {
            try
            {
                var result = await _collection.Find(filter).ToListAsync();

                //return bad request if no items were found
                if (result.Count == 0)
                {
                    return new CosmosDBErrorResult<List<T>?>(new List<string> { "No items found" }, CosmosDBResultCode.BadRequest);
                }

                return new CosmosDBSuccessResult<List<T>?>(new List<string> { }, result);

            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new CosmosDBErrorResult<List<T>?>(new List<string> { "Unexpected problem occured.", ex.Message }, CosmosDBResultCode.Error);
            }
        }

        public async Task<CosmosDBResult<T?>> GetItemAsync(Guid? id)
        {
            try
            {
                //check if paramter is null
                if (id == null)
                {
                    return new CosmosDBErrorResult<T?>(new List<string> { $"{nameof(id)} is null" }, CosmosDBResultCode.BadRequest);
                }

                var result = await _collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();

                //return bad request if no items were found
                if (result == null)
                {
                    return new CosmosDBErrorResult<T?>(new List<string> { "No item found" }, CosmosDBResultCode.BadRequest);
                }

                return new CosmosDBSuccessResult<T?>(new List<string> { }, result);
            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new CosmosDBErrorResult<T?>(new List<string> { "Unexpected problem occured.", ex.Message }, CosmosDBResultCode.Error);
            }
        }

        public async Task<CosmosDBResult<string?>> UpdateItemAsync(Guid? id, T? item)
        {
            try
            {
                //check parameters and return badrequest if any is null.
                List<string> responseMessage = new();

                if (id == null)
                {
                    responseMessage.Add($"{nameof(id)} is null");
                }
                if (item == null)
                {
                    responseMessage.Add($"{nameof(item)} is null");
                }
                if (responseMessage.Count > 0)
                {
                    return new CosmosDBErrorResult<string?>(responseMessage, CosmosDBResultCode.BadRequest);
                }

                var result = await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), item!);

                //return error if the result was not acknowledged
                if (!result.IsAcknowledged)
                {
                    return new CosmosDBErrorResult<string?>(new List<string> { "The result was not Acknowledged" }, CosmosDBResultCode.Error);
                }

                //return bad request if no items were found
                if (result.ModifiedCount == 0)
                {
                    return new CosmosDBErrorResult<string?>(new List<string> { "No items found to update" }, CosmosDBResultCode.BadRequest);
                }

                //return bad request if more than one item was updated
                if (result.ModifiedCount > 1)
                {
                    return new CosmosDBErrorResult<string?>(new List<string> { "More than one item was updated", $"{result.ModifiedCount} items was updated." }, CosmosDBResultCode.BadRequest);
                }

                return new CosmosDBEmptySuccessResult<string?>(new List<string> { });
            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new CosmosDBErrorResult<string?>(new List<string> { "Unexpected problem occured.", ex.Message }, CosmosDBResultCode.Error);
            }
        }
    }
}
