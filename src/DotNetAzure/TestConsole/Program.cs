using AzureServices.Services.OAuth2Token;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main()
        {
            HttpClient client = new();
            OAuth2TokenServiceConfiguration config = new("", "","", "");
            OAuth2TokenService tokenService = new(client, config);
            var result = await tokenService.GetTokenAsync();
            if(result.HasValue)
            {
                Console.WriteLine($"Token: {result.Value}");
            }
        }
    }
}