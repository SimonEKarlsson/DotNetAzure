namespace AzureServices.ExampleCode.apiManagement
{
    public interface IAPIM
    {
        Task GetApisAsync();
        Task GetOperationsAsync(string apis);
        Task GetOperationsAsyncOpenAIP(string apis);
        APIMModels ApimContent { get; }
        OperationModels Operations { get; }
    }
}