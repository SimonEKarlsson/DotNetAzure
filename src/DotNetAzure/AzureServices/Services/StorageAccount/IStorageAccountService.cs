namespace AzureServices.Services.StorageAccount
{
    /// <summary>
    /// Defines the contract for a storage account service.
    /// </summary>
    public interface IStorageAccountService
    {

        /// <summary>
        /// Creates a container in the storage account.
        /// </summary>
        /// <returns></returns>
        Task<StorageAccountResult<string?>> CreateContainerAsync();
        /// <summary>
        /// Uploads a file to the container.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="content">The streamcontent of the file.</param>
        /// <param name="overWrite">A bool which checks if it should overwrite the file if it already exists. overWrite = false is default and won't overwrite.</param>
        /// <returns></returns>
        Task<StorageAccountResult<string?>> UploadFile(string? fileName, Stream? content, bool overWrite = false);
        /// <summary>
        /// Gets the Stream of the file.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns></returns>
        Task<StorageAccountResult<Stream?>> ReadFileAsync(string? fileName);
        /// <summary>
        /// Deletes a file from the container.
        /// </summary>
        /// <param name="fileName">The name of he file that should be deleted.</param>
        /// <returns></returns>
        Task<StorageAccountResult<string?>> DeleteFileAsync(string? fileName);
        /// <summary>
        /// Lists all the files in the container.
        /// </summary>
        /// <returns></returns>
        Task<StorageAccountResult<IEnumerable<string>?>> ListContainerFiles();
    }
}
