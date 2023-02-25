using Azure.Messaging.ServiceBus;
using Mango.Services.OrderAPI.MessageBus;
using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumerOrder : IAzureServiceBusConsumerOrder
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionCheckout;
        private readonly string checkoutMessageTopic;
        private readonly string orderPaymentProcessTopic;
        private readonly string orderUpdatePaymentResultTopic;

        private readonly IConfiguration _configuration;
        private readonly OrderRepository _orderRepository;
        private ServiceBusProcessor checkoutProcessor;
        private ServiceBusProcessor orderUpadatePaymentStatusProcessor;

        private readonly IMessageBus _messageBus;

        public AzureServiceBusConsumerOrder(OrderRepository orderRepository, IConfiguration configuration, IMessageBus messageBus)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;
            _messageBus = messageBus;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionCheckout = _configuration.GetValue<string>("SubscriptionCheckout");
            checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");
            orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopics");
            orderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);
            checkoutProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionCheckout);
            orderUpadatePaymentStatusProcessor = client.CreateProcessor(orderUpdatePaymentResultTopic, subscriptionCheckout);
        }

        public async Task Start()
        {
            checkoutProcessor.ProcessMessageAsync += OnCheckoutMessageReceivedAsync;
            checkoutProcessor.ProcessErrorAsync += ErrorHeandler;
            await checkoutProcessor.StartProcessingAsync();

            orderUpadatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceivedAsync;
            orderUpadatePaymentStatusProcessor.ProcessErrorAsync += ErrorHeandler;
            await orderUpadatePaymentStatusProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await checkoutProcessor.StopProcessingAsync();
            await checkoutProcessor.DisposeAsync();

            await orderUpadatePaymentStatusProcessor.StopProcessingAsync();
            await orderUpadatePaymentStatusProcessor.DisposeAsync();
        }

        Task ErrorHeandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());

            return Task.CompletedTask;
        }

        private async Task OnCheckoutMessageReceivedAsync(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new()
            {
                UserId = checkoutHeaderDto.UserId,
                FirstName = checkoutHeaderDto.FirstName,
                LastName = checkoutHeaderDto.LastName,
                CardNumber = checkoutHeaderDto.CardNumber,
                CouponCode = checkoutHeaderDto.CouponCode,
                CVV = checkoutHeaderDto.CVV,
                DiscountTotal = checkoutHeaderDto.DiscountTotal,
                Email = checkoutHeaderDto.Email,
                ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderTotal = checkoutHeaderDto.OrderTotal,
                PaymentStatus = false,
                Phone = checkoutHeaderDto.Phone,
                PickupDateTime = checkoutHeaderDto.PickupDateTime
            };
            foreach (var detailList in checkoutHeaderDto.CartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detailList.ProductId,
                    ProductName = detailList.Product.Name,
                    Price = detailList.Product.Price,
                    Count = detailList.Count
                };
                orderHeader.CartTotalItems += detailList.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }

            await _orderRepository.AddOrderAsync(orderHeader);

            PaymentRequestMessage paymentRequestMessage = new()
            {
                Name = orderHeader.FirstName + " " + orderHeader.LastName,
                CardNumber = orderHeader.CardNumber,
                CVV = orderHeader.CVV,
                ExpiryMonth = orderHeader.ExpiryMonthYear,
                OrderId = orderHeader.OrderHeaderId,
                OrderTotal = orderHeader.OrderTotal,
                Email = orderHeader.Email,
            };

            try
            {
                await _messageBus.PublishMessageAsync(paymentRequestMessage, orderPaymentProcessTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnOrderPaymentUpdateReceivedAsync(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var updatePaymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            await _orderRepository.UpdateOrderPaymentStatus(updatePaymentResultMessage.OrderId, updatePaymentResultMessage.Status);
            await args.CompleteMessageAsync(args.Message);
        }
    }
}
