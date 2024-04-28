using DotNetAzure.UI.Models;

namespace DotNetAzure.UI.Pages
{
    public partial class CosmosDBUI
    {

        private async Task BtnCreateItem()
        {
            var result = await _cosmosDBService.AddItemAsync(new CosmosModel("Test"));
        }

        private async Task BtnUpdateItem()
        {
            var result = await _cosmosDBService.UpdateItemAsync(Guid.Parse("ad8b5972-62a3-47ba-bbc2-68e704bfb597"), new CosmosModel("bajs") { Id = Guid.Parse("ad8b5972-62a3-47ba-bbc2-68e704bfb597") });
        }

        private async Task BtnGetItem()
        {
            var result = await _cosmosDBService.GetItemAsync(Guid.Parse("21f46f13-5b20-4353-b44e-f603c7183768"));
        }

        private async Task BtnGetItems()
        {
            var result = await _cosmosDBService.GetItemsAsync();
        }

        private async Task BtnDeleteItem()
        {
            var result = await _cosmosDBService.DeleteItemAsync(Guid.Parse("21f46f13-5b20-4353-b44e-f603c7183768"));
        }
    }
}
