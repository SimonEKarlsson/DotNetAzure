using Newtonsoft.Json;

namespace AzureServices.Services.OAuth2Token
{
    public class OAuth2TokenService : IOAuth2TokenService
    {
        private readonly HttpClient _httpClient;
        private Auth2Token Auth2Token;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tenantId;
        private readonly string _scope;

        public OAuth2TokenService(HttpClient httpClient, string? clientId, string? clientSecret, string? tenantId, string? scope)
        {
            _httpClient = httpClient;
            Auth2Token = new Auth2Token();
            _clientId = clientId ?? throw new Exception($"{nameof(clientId)} can't be null");
            _clientSecret = clientSecret ?? throw new Exception($"{nameof(clientSecret)} can't be null");
            _tenantId = tenantId ?? throw new Exception($"{nameof(tenantId)} can't be null");
            _scope = scope ?? throw new Exception($"{nameof(scope)} can't be null");
        }

        public OAuth2TokenService(HttpClient httpClient, OAuth2TokenServiceConfiguration configuration)
        {
            _httpClient = httpClient;
            Auth2Token = new Auth2Token();
            _clientId = configuration.ClientId!;
            _clientSecret = configuration.ClientSecret!;
            _tenantId = configuration.TenantId!;
            _scope = configuration.Scope!;
        }

        private bool IsTokenExpired(int expirationThresholdSeconds = 60)
        {
            //check if token is null or empty
            if (Auth2Token == null || string.IsNullOrEmpty(Auth2Token.Access_Token))
            {
                return true;
            }

            // Split the token into its parts and check if it is well-formed
            string[] tokenParts = Auth2Token.Access_Token.Split('.');
            if (tokenParts.Length < 2)
            {
                return true;
            }

            // Decode the payload and extract the expiration time
            string tokenPayload = tokenParts[1];
            string decodedTokenPayload = DecodeToken(tokenPayload);
            dynamic? tokenData = JsonConvert.DeserializeObject(decodedTokenPayload);

            // Check if the token data is null
            if (tokenData == null)
            {
                return true;
            }

            long expirationTimeUnix = tokenData.exp;

            // Convert expiration time from Unix timestamp to DateTime
            DateTime expirationDateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(expirationTimeUnix).UtcDateTime;

            // Check if the token has expired or is close to expiring
            DateTime currentDateTimeUtc = DateTime.UtcNow;
            TimeSpan timeUntilExpiration = expirationDateTimeUtc - currentDateTimeUtc;

            // If the time until expiration is less than the threshold, consider it close to expiring
            bool isCloseToExpiring = timeUntilExpiration.TotalSeconds <= expirationThresholdSeconds;

            // Return true if the token has expired or is close to expiring
            return expirationDateTimeUtc <= currentDateTimeUtc || isCloseToExpiring;
        }

        // Helper method to decode Base64Url encoded strings
        private static string DecodeToken(string input)
        {
            string base64 = input.Replace('-', '+').Replace('_', '/');
            string padding = new('=', (4 - base64.Length % 4) % 4);
            string base64Url = base64 + padding;
            byte[] bytes = Convert.FromBase64String(base64Url);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        private async Task UpdateTokenAsync()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", $"{_clientId}" },
                { "client_secret", $"{_clientSecret}" },
                { "scope", $"{_scope}" }
            });

            var response = await _httpClient.PostAsync($"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Auth2Token? auth2Token = JsonConvert.DeserializeObject<Auth2Token>(responseContent);
                if (auth2Token != null && !string.IsNullOrEmpty(auth2Token.Access_Token))
                {
                    Auth2Token = auth2Token;
                }
                else
                {
                    //if new token is still null or expired, reset token
                    Auth2Token = new();
                }
            }
            else
            {
                //if token is still null or expired, reset token
                Auth2Token = new();
            }
        }

        public async Task<OAuth2TokenResult<string?>> GetTokenAsync()
        {
            try
            {
                //check if token is expired
                if (IsTokenExpired())
                {
                    await UpdateTokenAsync();
                }

                //if token is still null or expired, return error
                if (IsTokenExpired())
                {
                    return new OAuth2TokenErrorResult<string?>(new List<string> { "Error getting OAuth2 Token." }, OAuth2TokenResultCode.Error);
                }

                return new OAuth2TokenSuccessResult<string?>(new List<string> { }, Auth2Token.Access_Token);
            }
            catch (Exception e)
            {
                return new OAuth2TokenErrorResult<string?>(new List<string> { "Error getting OAuth2 Token.", e.Message }, OAuth2TokenResultCode.Error);
            }
        }

        private bool TokenHasValidRole(string role)
        {
            //check if token is null or empty
            if (Auth2Token == null || string.IsNullOrEmpty(Auth2Token.Access_Token))
            {
                return false;
            }

            // Split the token into its parts and check if it is well-formed
            string[] tokenParts = Auth2Token.Access_Token.Split('.');
            if (tokenParts.Length < 2)
            {
                return false;
            }

            // Decode the payload and extract the expiration time
            string tokenPayload = tokenParts[1];
            string decodedTokenPayload = DecodeToken(tokenPayload);
            dynamic? tokenData = JsonConvert.DeserializeObject(decodedTokenPayload);

            // Check if the token data is null
            if (tokenData == null)
            {
                return false;
            }

            List<string> roles = tokenData.roles.ToObject<List<string>>();

            // Check if the token has the required role
            return roles.Contains(role);
        }

        public async Task<OAuth2TokenResult<bool>> CheckRole(string role)
        {
            try
            {
                //check if token is expired
                if (IsTokenExpired())
                {
                    await UpdateTokenAsync();
                }

                //if token is still null or expired, return error
                if (IsTokenExpired())
                {
                    return new OAuth2TokenErrorResult<bool>(new List<string> { "Error getting OAuth2 Token." }, OAuth2TokenResultCode.Error);
                }

                return new OAuth2TokenSuccessResult<bool>(new List<string> { }, TokenHasValidRole(role));
            }
            catch (Exception e)
            {
                return new OAuth2TokenErrorResult<bool>(new List<string> { "Error getting OAuth2 Token and Checking the role.", e.Message }, OAuth2TokenResultCode.Error);
            }
        }
    }

    public class Auth2Token
    {
        public string Token_Type { get; set; } = string.Empty;
        public int Expires_In { get; set; } = 0;
        public int Ext_Expires_In { get; set; } = 0;
        public string Access_Token { get; set; } = string.Empty;
    }

    public class OAuth2TokenServiceConfiguration
    {
        public string ClientId { get; private set; } = string.Empty;
        public string ClientSecret { get; private set; } = string.Empty;
        public string TenantId { get; private set; } = string.Empty;
        public string Scope { get; private set; } = string.Empty;

        public OAuth2TokenServiceConfiguration(string? clientId, string? clientSecret, string? tenantId, string? scope)
        {
            ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
            TenantId = tenantId ?? throw new ArgumentNullException(nameof(tenantId));
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }
    }
}
