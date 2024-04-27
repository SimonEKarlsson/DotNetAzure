namespace AzureServices.Services.StorageAccount
{
    /// <summary>
    /// Defines the contract for a storage account service.
    /// </summary>
    public interface IStorageAccountService
    {

        /// <summary>
        /// Creates a new container if it does not already exist.
        /// </summary>
        Task<StorageAccountResult<string?>> CreateContainerAsync();
        Task<StorageAccountResult<string?>> UploadFile(string? fileName, Stream? content, bool overWrite = false);
        Task<StorageAccountResult<Stream?>> ReadFileAsync(string? fileName);
        Task<StorageAccountResult<string?>> DeleteFileAsync(string? fileName);
        Task<StorageAccountResult<IEnumerable<string>?>> ListContainerFiles();
    }
}
