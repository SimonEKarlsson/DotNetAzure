namespace AzureServices.Services.ServiceBus
{
    /// <summary>
    /// Defines a service interface for interacting with an Azure Service Bus.
    /// </summary>
    public interface IServiceBusService
    {
        /// <summary>
        /// Sends a message to the Azure Service Bus.
        /// </summary>
        /// <param name="message">The message content to be sent. Must not be null or empty.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation, containing a <see cref="ServiceBusResult{T}"/> 
        /// which encapsulates the result of the send operation, including success or error information.
        /// </returns>
        /// <remarks>
        /// This method serializes the message content as a string and sends it to the configured Service Bus queue.
        /// If the message content is null or empty, the method returns a BadRequest result without sending the message.
        /// </remarks>
        Task<ServiceBusResult<string>> SendMessageAsync(string? message);

        /// <summary>
        /// Receives a message from the Azure Service Bus.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation, containing a <see cref="ServiceBusResult{T}"/>
        /// which encapsulates the result of the receive operation. This includes either the message content, 
        /// a message indicating no content was available, or an error.
        /// </returns>
        /// <remarks>
        /// This method attempts to receive a message from the Service Bus queue within a predefined timeout period.
        /// If a message is received, it is returned as part of a successful result. If no message is received within the timeout,
        /// the method returns a NoContent result. If an error occurs during the operation, an Error result is returned.
        /// </remarks>
        Task<ServiceBusResult<string>> ReceiveMessageAsync();

        Task<ServiceBusResult<string>> ScheduleMessageAsync(string message, DateTimeOffset scheduleEnqueueTime);
        Task<ServiceBusResult<bool>> CancelScheduledMessageAsync(long sequenceNumber);
        Task<ServiceBusResult<string>> PeekMessageAsync();
        Task<ServiceBusResult<bool>> CompleteMessageAsync(string lockToken);
        Task<ServiceBusResult<bool>> AbandonMessageAsync(string lockToken);
        Task<ServiceBusResult<bool>> DeadLetterMessageAsync(string lockToken);
        Task<ServiceBusResult<List<string>>> ReceiveBatchMessagesAsync(int maxMessageCount, TimeSpan maxWaitTime);
    }
}