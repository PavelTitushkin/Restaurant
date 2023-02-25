namespace Mango.Services.ShoppingCartAPI.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessageAsync(BaseMessage message, string topicName);
    }
}
