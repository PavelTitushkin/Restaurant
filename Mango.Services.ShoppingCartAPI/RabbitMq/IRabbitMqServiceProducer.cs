namespace Mango.Services.OrderAPI.RabbitMQ
{
    public interface IRabbitMqServiceProducer
    {
        Task PublishMessageAsync(BaseMessageRabbitMq message, string queueName);
    }
}
