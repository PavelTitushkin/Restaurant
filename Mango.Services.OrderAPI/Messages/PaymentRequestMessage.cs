using Mango.Services.OrderAPI.MessageBus;

namespace Mango.Services.OrderAPI.Messages
{
    public class PaymentRequestMessage : BaseMessage
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonth { get; set; }
        public double OrderTotal { get; set; }
    }
}
