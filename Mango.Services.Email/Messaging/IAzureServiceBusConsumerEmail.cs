namespace Mango.Services.Email.Messaging
{
    public interface IAzureServiceBusConsumerEmail
    {
        Task Start();
        Task Stop();
    }
}
