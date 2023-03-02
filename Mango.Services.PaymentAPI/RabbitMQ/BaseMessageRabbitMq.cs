namespace Mango.Services.PaymentAPI.RabbitMQ
{ 
    public class BaseMessageRabbitMq
    {
        public int Id { get; set; }
        public DateTime MessageCreated { get; set; }
    }
}
