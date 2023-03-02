using Mango.Services.PaymentAPI.RabbitMQ;

namespace Mango.Services.PaymentAPI.Messages
{
    public class UpdatePaymentResultMessage : BaseMessageRabbitMq
    {
        public int OrderId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
