using AzureServices.Services.StorageAccount;

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
    }
}
