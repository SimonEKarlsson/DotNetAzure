using AzureServices.Services.OAuth2Token;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main()
        {
            HttpClient client = new();
            OAuth2TokenService tokenService = new(client, "", "", "", "");
            var result = await tokenService.GetTokenAsync();
            if(result.HasValue)
            {
                Console.WriteLine($"Token: {result.Value}");
            }

            var roleResult = await tokenService.CheckRole("Token.Read");
            if (roleResult.HasValue)
            {
                Console.WriteLine($"Role: {roleResult.Value}");
            }
        }
    }
}