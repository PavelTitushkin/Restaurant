namespace Mango.Services.OrderAPI.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessageAsync(BaseMessage message, string topicName);
    }
}
