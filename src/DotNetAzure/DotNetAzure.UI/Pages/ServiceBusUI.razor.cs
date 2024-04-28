namespace DotNetAzure.UI.Pages
{
    public partial class ServiceBusUI
    {
        private async Task BtnSendMessage()
        {
            await SB.SendMessageAsync("Första testet");
        }

        private async Task BtnReceiveMessage()
        {
            var result = await SB.ReceiveMessageAsync();
        }
    }
}
