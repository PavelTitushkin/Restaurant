using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.ShoppingCartAPI.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        //add serviceBus
        private readonly string connectingString = "...servicebus.windows.com...";
        public async Task PublishMessageAsync(BaseMessage message, string topicName)
        {
            await using var client = new ServiceBusClient(connectingString);

            ServiceBusSender sender = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);
            var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);
            await sender.DisposeAsync();
        }
    }
}
