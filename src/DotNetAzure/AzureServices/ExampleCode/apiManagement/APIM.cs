using Azure.Core;
using Azure.Identity;
using Newtonsoft.Json;

namespace AzureServices.ExampleCode.apiManagement
{
    public class APIM : IAPIM
    {
        private readonly HttpClient _client;
        private readonly string _subscriptionId;
        private readonly string _resourceGroupName;
        private readonly string _apimName;
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private string _token = string.Empty;
        private AccessToken AccessToken;
        public APIM(HttpClient client, ApimConfig config)
        {
            _client = client;
            _subscriptionId = config.SubscriptionId;
            _resourceGroupName = config.ResourceGroupName;
            _apimName = config.ApimName;
            _clientSecret = config.ClientSecret;
            _tenantId = config.TenantId;
            _clientId = config.ClientId;
        }

        public APIMModels ApimContent { get; private set; } = new();
        public OperationModels Operations { get; private set; } = new();
        private async Task GetTokenAsync()
        {
            string authority = $"https://login.microsoftonline.com/{_tenantId}";
            string scope = "https://management.azure.com/.default";
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{authority}/oauth2/v2.0/token");
            tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = _clientId,
                ["client_secret"] = _clientSecret,
                ["scope"] = scope
            });
            var tokenResponse = await _client.SendAsync(tokenRequest);
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Token>(tokenContent);
            Console.WriteLine(token.Access_Token);
            _token = token.Access_Token;
        }

        private async Task GetDefaultAzureCredentialAsync()
        {
            if(!string.IsNullOrEmpty(AccessToken.Token) && AccessToken.ExpiresOn > DateTimeOffset.UtcNow)
            {
                return;
            }

            AccessToken = await new DefaultAzureCredential().GetTokenAsync(new TokenRequestContext(new[] { "https://management.azure.com/.default" }));
            Console.WriteLine(AccessToken.Token);
            Console.WriteLine(AccessToken.ExpiresOn);
        }
        private void SetHeaders()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken.Token}");
        }
        public async Task GetApisAsync()
        {
            await GetDefaultAzureCredentialAsync();
            //await GetTokenAsync();
            SetHeaders();
            string apimUrl = $"https://management.azure.com/subscriptions/{_subscriptionId}/resourceGroups/{_resourceGroupName}/providers/Microsoft.ApiManagement/service/{_apimName}/apis?api-version=2022-08-01";
            var response = await _client.GetAsync(apimUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ApimContent = JsonConvert.DeserializeObject<APIMModels>(content)!;
            }
            else
            {
                ApimContent = new();
            }
        }
        public async Task GetOperationsAsync(string apis)
        {
            //await GetTokenAsync();
            await GetDefaultAzureCredentialAsync();
            SetHeaders();
            string apimUrl = $"https://management.azure.com/subscriptions/{_subscriptionId}/resourceGroups/{_resourceGroupName}/providers/Microsoft.ApiManagement/service/{_apimName}/apis/{apis}/operations?api-version=2022-08-01";
            var response = await _client.GetAsync(apimUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Operations = JsonConvert.DeserializeObject<OperationModels>(content)!;
            }
            else
            {
                Operations = new();
            }
        }

        public async Task GetOperationsAsyncOpenAIP(string apis)
        {
            //await GetTokenAsync();
            await GetDefaultAzureCredentialAsync();
            SetHeaders();
            string apimUrl = $"https://management.azure.com/subscriptions/{_subscriptionId}/resourceGroups/{_resourceGroupName}/providers/Microsoft.ApiManagement/service/{_apimName}/apis/{apis}?format=openapi-link&export=true&api-version=2022-08-01";
            var response = await _client.GetAsync(apimUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Operations = new();
            }
        }
    }

    public class Token
    {
        public string Token_Type { get; set; } = string.Empty;
        public string Access_Token { get; set; } = string.Empty;
        public int Expires_In { get; set; } = 0;
        public int Ext_Expires_In { get; set; } = 0;
    }

    public class ApimConfig
    {
        public string SubscriptionId { get; private set; }
        public string ResourceGroupName { get; private set; }
        public string ApimName { get; private set; }
        public string TenantId { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        public ApimConfig(string subscriptionId, string resourceGroupName, string apimName, string tenantId, string clientId, string clientSecret)
        {
            SubscriptionId = subscriptionId;
            ResourceGroupName = resourceGroupName;
            ApimName = apimName;
            TenantId = tenantId;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}
