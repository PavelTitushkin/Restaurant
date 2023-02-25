namespace Mango.Services.PaymentAPI.Messaging
{
    public interface IAzureServiceBusConsumerPayment
    {
        Task Start();
        Task Stop();
    }
}
