namespace Mango.Services.PaymentAPI.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessageAsync(BaseMessage message, string topicName);
    }
}
