using AzureServices.Services.OAuth2Token;
using SharpCompress.Common;

namespace AzureServices.Services.Http
{
    public class SharepointHttpClient : BaseHttpClient
    {
        private readonly string _baseUrl;
        private readonly OAuth2TokenService _OAuth2TokenService; //requires OAuth2TokenService
        private readonly string _driveId = "";

        public SharepointHttpClient(HttpClient httpclient, string baseUrl, OAuth2TokenServiceConfiguration tokenConfig, string driveId) : base(httpclient)
        {
            _baseUrl = baseUrl;
            _OAuth2TokenService = new OAuth2TokenService(httpclient, tokenConfig);
            _driveId = driveId;
        }

        internal override async Task CreateAuthHeaderAsync()
        {
            var tokenResponse = await _OAuth2TokenService.GetTokenAsync();

            if (tokenResponse.StatusCode == OAuth2TokenResultCode.OK)
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("Authorization", tokenResponse.Value);
            }
            else
            {
                Console.WriteLine(tokenResponse.StringMessages);
            }
        }

        public async Task<BaseHttpResult<HttpResponseMessage>> UploadFile(string fileName, byte[] fileContent)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return new BaseHttpErrorResult<HttpResponseMessage>(new() { $"{nameof(fileName)} is null or empty" }, System.Net.HttpStatusCode.BadRequest);
                }

                if (fileContent == null || fileContent.Length == 0)
                {
                    return new BaseHttpErrorResult<HttpResponseMessage>(new() { $"{nameof(fileContent)} is null or empty" }, System.Net.HttpStatusCode.BadRequest);
                }

                //var url = $"{_baseUrl}/sites/{fileName}/drive/root:/{fileName}:/content";
                var url = $"{_baseUrl}/{fileName}";
                var content = new ByteArrayContent(fileContent);
                return await PostAsync(url, content);
            }
            catch (Exception ex)
            {
                return new BaseHttpErrorResult<HttpResponseMessage>(new() { ex.Message }, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseHttpResult<HttpResponseMessage>> DownloadFile(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return new BaseHttpErrorResult<HttpResponseMessage>(new() { $"{nameof(fileName)} is null or empty" }, System.Net.HttpStatusCode.BadRequest);
                }
                //var url = $"{_baseUrl}/sites/{fileName}/drive/root:/{fileName}:/content";
                var url = $"{_baseUrl}/{fileName}";
                return await GetAsync(url);
            }
            catch (Exception ex)
            {
                return new BaseHttpErrorResult<HttpResponseMessage>(new() { ex.Message }, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseHttpResult<HttpResponseMessage>> GetFile(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return new BaseHttpErrorResult<HttpResponseMessage>(new() { $"{nameof(fileName)} is null or empty" }, System.Net.HttpStatusCode.BadRequest);
                }
                //var url = $"{_baseUrl}/sites/{fileName}/drive/root:/{fileName}:/content";
                var url = $"{_baseUrl}/v1.0/drives/{_driveId}/root:/{fileName}/?expand=listItem";
                return await GetAsync(url);
            }
            catch (Exception ex)
            {
                return new BaseHttpErrorResult<HttpResponseMessage>(new() { ex.Message }, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
