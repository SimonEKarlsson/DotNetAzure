using AzureServices.Services.CosmosDB;
using AzureServices.Services.StorageAccount;
using DotNetAzure.UI.Models;

namespace DotNetAzure.UI
{
    public static class Setup
    {
        public static void StorageContainerSetup(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration["StorageAccount:connectionString"] ?? throw new Exception("StorageAccount:connectionString is missing");
            var container = builder.Configuration["StorageAccount:container"] ?? throw new Exception("StorageAccount:container is missing");

            builder.Services.AddScoped<IStorageAccountService, StorageAccountService>(options =>
            {
                return new StorageAccountService(connectionString, container);
            });
        }

        public static void CosmosDBSetup(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration["CosmosDB:connectionString"] ?? throw new Exception("CosmosDB:connectionString is missing");
            var database = builder.Configuration["CosmosDB:database"] ?? throw new Exception("CosmosDB:database is missing");
            var collection = builder.Configuration["CosmosDB:collection"] ?? throw new Exception("CosmosDB:collection is missing");

            builder.Services.AddScoped<ICosmosDBService<CosmosModel>, CosmosDBService<CosmosModel>>(options =>
            {
                return new CosmosDBService<CosmosModel>(connectionString, database, collection);
            });
        }
    }
}
