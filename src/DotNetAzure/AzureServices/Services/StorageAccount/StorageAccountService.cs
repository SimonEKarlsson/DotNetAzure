using Azure;
using Azure.Storage.Blobs;

namespace AzureServices.Services.StorageAccount
{
    public class StorageAccountService : IStorageAccountService
    {
        // Nuget: Azure.Storage.Blobs 12.19.1
        private readonly BlobServiceClient _client;
        private readonly BlobContainerClient _containerClient;
        private readonly string _containerName;
        public StorageAccountService(string connectionString, string container)
        {
            _client = new BlobServiceClient(connectionString ?? throw new ArgumentNullException(nameof(connectionString), "ConnectionString is missing"));

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container), "Container is missing");
            }
            _containerName = container;
            _containerClient = _client.GetBlobContainerClient(container);
        }

        public async Task<StorageAccountResult<string?>> CreateContainerAsync()
        {
            try
            {
                //checks if the container exists
                if (await _containerClient.ExistsAsync())
                {
                    //does a return if it already exists.
                    return new StorageAccountErrorResult<string?>(new List<string> { $"{_containerName} already exists" }, StorageAccountResultCode.BadRequset);
                }

                //creates the container.
                var result = await _containerClient.CreateIfNotExistsAsync();
                var httpResponse = result.GetRawResponse();

                //201 means the container was created and was ok
                if (httpResponse.Status == 201)
                {
                    return new StorageAccountEmptySuccessResult<string?>(new List<string> { });
                }
                //unexpected error occured. The statuscode and reasonPhrase is sent back.
                return new StorageAccountErrorResult<string?>(new List<string> { $"Nonaccepted statuscode for creating {_containerName}.", httpResponse.Status.ToString(), httpResponse.ReasonPhrase }, StorageAccountResultCode.Error);
            }
            //RequestFailedException occures when something goes wrong with CreateIfNotExistsAsync(); 
            catch (RequestFailedException ex)
            {
                return new StorageAccountErrorResult<string?>(new List<string> { "Problem with CreateIfNotExistsAsync().", ex.Message }, StorageAccountResultCode.Error);
            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new StorageAccountErrorResult<string?>(new List<string> { "Unexpected problem occured.", ex.Message }, StorageAccountResultCode.Error);
            }
        }

        public async Task<StorageAccountResult<string?>> UploadFile(string? fileName, Stream? content, bool overWrite = false)
        {
            try
            {
                //check parameters and return badrequest if any is null.
                List<string> responseMessage = new();
                if (string.IsNullOrEmpty(fileName))
                {
                    responseMessage.Add($"{nameof(fileName)} is null");
                }
                if (content == null || content == Stream.Null)
                {
                    responseMessage.Add($"{nameof(content)} is null");
                }
                if (responseMessage.Count > 0)
                {
                    return new StorageAccountErrorResult<string?>(responseMessage, StorageAccountResultCode.BadRequset);
                }

                var blobClient = _containerClient.GetBlobClient(fileName);

                //if overwrite = false and the blob exists, return badrequest
                if (!overWrite && await blobClient.ExistsAsync())
                {
                    return new StorageAccountErrorResult<string?>(new List<string> { $"{nameof(overWrite)} is set to false and blob exists" }, StorageAccountResultCode.BadRequset);
                }

                var result = await blobClient.UploadAsync(content, overWrite);
                var httpResponse = result.GetRawResponse();

                //201 means the file was uploaded and was ok
                if (httpResponse.Status == 201)
                {
                    return new StorageAccountEmptySuccessResult<string?>(new List<string> { });
                }

                //unexpected error occured. The statuscode and reasonPhrase is sent back.
                return new StorageAccountErrorResult<string?>(new List<string> { $"Nonaccepted statuscode for uploading {fileName}.", httpResponse.Status.ToString(), httpResponse.ReasonPhrase }, StorageAccountResultCode.Error);
            }
            //RequestFailedException occures when something goes wrong with UploadAsync(); 
            catch (RequestFailedException ex)
            {
                return new StorageAccountErrorResult<string?>(new List<string> { "Problem with UploadAsync().", ex.Message }, StorageAccountResultCode.Error);
            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new StorageAccountErrorResult<string?>(new List<string> { "Unexpected problem occured.", ex.Message }, StorageAccountResultCode.Error);
            }
        }

        public async Task<StorageAccountResult<Stream?>> ReadFileAsync(string? fileName)
        {
            try
            {
                //check parameters and return badrequest if any is null.
                if (string.IsNullOrEmpty(fileName))
                {
                    return new StorageAccountErrorResult<Stream?>(new List<string> { $"{nameof(fileName)} is null" }, StorageAccountResultCode.BadRequset);
                }

                //if the blob doesn't exist, return bad request.
                var blobClient = _containerClient.GetBlobClient(fileName);
                if (!await blobClient.ExistsAsync())
                {
                    return new StorageAccountErrorResult<Stream?>(new List<string> { $"the blob {fileName} doesn't exists" }, StorageAccountResultCode.BadRequset);
                }

                var result = await blobClient.DownloadAsync();
                var httpResponse = result.GetRawResponse();

                //200 means ok
                if (httpResponse.Status == 200)
                {
                    return new StorageAccountSuccessResult<Stream?>(new List<string> { }, result.Value.Content);
                }

                //unexpected error occured. The statuscode and reasonPhrase is sent back.
                return new StorageAccountErrorResult<Stream?>(new List<string> { $"Nonaccepted statuscode for downloading {fileName}.", httpResponse.Status.ToString(), httpResponse.ReasonPhrase }, StorageAccountResultCode.Error);

            }
            //RequestFailedException occures when something goes wrong with DownloadAsync(); 
            catch (RequestFailedException ex)
            {
                return new StorageAccountErrorResult<Stream?>(new List<string> { "Problem with DownloadAsync().", ex.Message }, StorageAccountResultCode.Error);
            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new StorageAccountErrorResult<Stream?>(new List<string> { "Unexpected problem occured.", ex.Message }, StorageAccountResultCode.Error);
            }
        }

        public async Task<StorageAccountResult<string?>> DeleteFileAsync(string? fileName)
        {
            try
            {
                //check parameters and return badrequest if any is null.
                if (string.IsNullOrEmpty(fileName))
                {
                    return new StorageAccountErrorResult<string?>(new List<string> { $"{nameof(fileName)} is null" }, StorageAccountResultCode.BadRequset);
                }

                //if the blob doesn't exist, return bad request.
                var blobClient = _containerClient.GetBlobClient(fileName);
                if (!await blobClient.ExistsAsync())
                {
                    return new StorageAccountErrorResult<string?>(new List<string> { $"the blob {fileName} doesn't exists" }, StorageAccountResultCode.BadRequset);
                }

                var result = await blobClient.DeleteAsync();

                //202 means the file was deleted
                if (result.Status == 202)
                {
                    return new StorageAccountEmptySuccessResult<string?>(new List<string> { });
                }

                //unexpected error occured. The statuscode and reasonPhrase is sent back.
                return new StorageAccountErrorResult<string?>(new List<string> { $"Nonaccepted statuscode for deleting {fileName}.", result.Status.ToString(), result.ReasonPhrase }, StorageAccountResultCode.Error);

            }
            //RequestFailedException occures when something goes wrong with DeleteIfExistsAsync(); 
            catch (RequestFailedException ex)
            {
                return new StorageAccountErrorResult<string?>(new List<string> { "Problem with DeleteIfExistsAsync().", ex.Message }, StorageAccountResultCode.Error);
            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new StorageAccountErrorResult<string?>(new List<string> { "Unexpected problem occured.", ex.Message }, StorageAccountResultCode.Error);
            }
        }

        public async Task<StorageAccountResult<IEnumerable<string>?>> ListContainerFiles()
        {
            try
            {
                //list for all the blobs
                var blobs = new List<string>();

                //loop through all the blobs
                await foreach (var blob in _containerClient.GetBlobsAsync())
                {
                    blobs.Add(blob.Name);
                }

                //if list is empty
                if (blobs.Count == 0)
                {
                    return new StorageAccountEmptySuccessResult<IEnumerable<string>?>(new List<string> { });
                }

                //if list contains items
                if (blobs.Count > 0)
                {
                    return new StorageAccountSuccessResult<IEnumerable<string>?>(new List<string> { }, blobs);
                }
                return new StorageAccountErrorResult<IEnumerable<string>?>(new List<string> { $"Something went wrong with getting blobs from {_containerName}" }, StorageAccountResultCode.Error);
            }
            //RequestFailedException occures when something goes wrong with GetBlobsAsync(); 
            catch (RequestFailedException ex)
            {
                return new StorageAccountErrorResult<IEnumerable<string>?>(new List<string> { "Problem with GetBlobsAsync().", ex.Message }, StorageAccountResultCode.Error);
            }
            //the rest of the exceptions
            catch (Exception ex)
            {
                return new StorageAccountErrorResult<IEnumerable<string>?>(new List<string> { "Unexpected problem occured.", ex.Message }, StorageAccountResultCode.Error);
            }
        }
    }
}
