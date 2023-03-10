using Azure.Messaging.ServiceBus;
using Mango.Services.PaymentAPI.MessageBus;
using Mango.Services.PaymentAPI.Messages;
using Newtonsoft.Json;
using PaymentProcessor;
using System.Text;

namespace Mango.Services.PaymentAPI.Messaging
{
    //public class AzureServiceBusConsumerPayment : IAzureServiceBusConsumerPayment
    //{
    //    private readonly string serviceBusConnectionString;
    //    private readonly string subscriptionPayment;
    //    private readonly string orderPaymentProcessTopic;
    //    private readonly string orderUpdatePaymentResultTopic;

    //    private ServiceBusProcessor orderPaymentProcessor;
    //    private readonly IProcessorPayment _processorPayment;
    //    private readonly IConfiguration _configuration;
    //    private readonly IMessageBus _messageBus;

    //    public AzureServiceBusConsumerPayment(IConfiguration configuration, IMessageBus messageBus, IProcessorPayment processorPayment)
    //    {
    //        _processorPayment = processorPayment;
    //        _configuration = configuration;
    //        _messageBus = messageBus;

    //        serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
    //        subscriptionPayment = _configuration.GetValue<string>("OrderPaymentProcessSubscription");
    //        orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopics");
    //        orderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");

    //        var client = new ServiceBusClient(serviceBusConnectionString);

    //        orderPaymentProcessor = client.CreateProcessor(orderPaymentProcessTopic, subscriptionPayment);
    //    }

    //    public async Task Start()
    //    {
    //        orderPaymentProcessor.ProcessMessageAsync += ProcessPaymentsAsync;
    //        orderPaymentProcessor.ProcessErrorAsync += ErrorHeandler;
    //        await orderPaymentProcessor.StartProcessingAsync();
    //    }

    //    public async Task Stop()
    //    {
    //        await orderPaymentProcessor.StopProcessingAsync();
    //        await orderPaymentProcessor.DisposeAsync();
    //    }

    //    Task ErrorHeandler(ProcessErrorEventArgs args)
    //    {
    //        Console.WriteLine(args.Exception.ToString());

    //        return Task.CompletedTask;
    //    }

    //    private async Task ProcessPaymentsAsync(ProcessMessageEventArgs args)
    //    {
    //        var message = args.Message;
    //        var body = Encoding.UTF8.GetString(message.Body);

    //        var paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);
    //        var result = _processorPayment.PaymentProcessor();

    //        UpdatePaymentResultMessage updatePaymentResultMessage = new()
    //        {
    //            Status = result,
    //            OrderId = paymentRequestMessage.OrderId,
    //            Email = paymentRequestMessage.Email
    //        };

    //        try
    //        {
    //            await _messageBus.PublishMessageAsync(updatePaymentResultMessage, orderUpdatePaymentResultTopic);
    //            await args.CompleteMessageAsync(args.Message);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw;
    //        }
    //    }
    //}
}
