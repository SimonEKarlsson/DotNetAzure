using AzureServices.Services.OAuth2Token;

namespace AzureServices.Services.Sharepoint
{
    public class SharepointService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        private readonly OAuth2TokenService _Token; //requires OAuth2TokenService
        public SharepointService(HttpClient httpclient, OAuth2TokenServiceConfiguration tokenConfig)
        {
            _client = httpclient;
            _baseUrl = "https://graph.microsoft.com/";
            _Token = new OAuth2TokenService(httpclient, tokenConfig);
        }

        private async Task Temp()
        {
            Console.WriteLine(_client);
            Console.WriteLine(_baseUrl);
            var token = await _Token.GetTokenAsync();
            Console.WriteLine(token);
        }
    }
}
