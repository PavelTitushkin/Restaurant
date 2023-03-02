namespace Mango.Services.OrderAPI.RabbitMQ
{ 
    public class BaseMessageRabbitMq
    {
        public int Id { get; set; }
        public DateTime MessageCreated { get; set; }
    }
}
