namespace Mango.Services.PaymentAPI.RabbitMQ
{
    public interface IRabbitMqServiceProducer
    {
        void PublishMessageAsync(BaseMessageRabbitMq message);
    }
}
