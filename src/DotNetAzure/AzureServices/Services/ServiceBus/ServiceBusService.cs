using Azure.Messaging.ServiceBus;

namespace AzureServices.Services.ServiceBus
{
    public class ServiceBusService : IServiceBusService
    {
        //Azure.Messaging.ServiceBus 7.17.5
        private readonly ServiceBusClient _client;
        private readonly string _queueName;

        public ServiceBusService(string? connectionString, string? queueName)
        {
            if (string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentException($"{nameof(queueName)} cannot be null or empty", nameof(queueName));
            }
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"{nameof(connectionString)} cannot be null or empty", nameof(connectionString));
            }

            _client = new ServiceBusClient(connectionString);
            _queueName = queueName;
        }

        public async Task<ServiceBusResult<string>> SendMessageAsync(string? message)
        {
            try
            {
                // Validate message
                if (string.IsNullOrEmpty(message))
                {
                    return new ServiceBusErrorResult<string>(new List<string> { $"{nameof(message)} cannot be null or empty" }, ServiceBusResultCode.BadRequest);
                }

                // Send message
                await using (ServiceBusSender sender = _client.CreateSender(_queueName))
                {
                    ServiceBusMessage serviceBusMessage = new(message);
                    await sender.SendMessageAsync(serviceBusMessage);
                }
                return new ServiceBusSuccessResult<string>(new List<string> { "Message sent successfully" }, message);
            }
            //catch potential problems
            catch (Exception ex)
            {
                return new ServiceBusErrorResult<string>(new List<string> { ex.Message }, ServiceBusResultCode.Error);
            }
        }

        public async Task<ServiceBusResult<string>> ReceiveMessageAsync()
        {
            try
            {
                await using (ServiceBusReceiver receiver = _client.CreateReceiver(_queueName))
                {
                    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(10));
                    if (receivedMessage != null)
                    {
                        string messageBody = receivedMessage.Body.ToString();
                        await receiver.CompleteMessageAsync(receivedMessage);
                        return new ServiceBusSuccessResult<string>(new List<string> { }, messageBody);
                    }
                    else
                    {
                        return new ServiceBusEmptySuccessResult<string>(new List<string> { });
                    }
                }
            }
            //catch potential problems
            catch (Exception ex)
            {
                return new ServiceBusErrorResult<string>(new List<string> { ex.Message }, ServiceBusResultCode.Error);
            }
        }
    }
}
