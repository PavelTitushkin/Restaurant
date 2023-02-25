namespace Mango.Services.OrderAPI.Messaging
{
    public interface IAzureServiceBusConsumerOrder
    {
        Task Start();
        Task Stop();
    }
}
