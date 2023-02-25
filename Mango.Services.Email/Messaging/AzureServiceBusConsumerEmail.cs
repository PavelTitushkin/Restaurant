using Azure.Messaging.ServiceBus;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Email.Messaging
{
    public class AzureServiceBusConsumerEmail : IAzureServiceBusConsumerEmail
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionEmail;
        private readonly string orderUpdatePaymentResultTopic;

        private readonly IConfiguration _configuration;

        private readonly EmailRepository _emailRepository;

        private ServiceBusProcessor orderUpadatePaymentStatusProcessor;

        public AzureServiceBusConsumerEmail(EmailRepository emailRepository, IConfiguration configuration)
        {
            _emailRepository = emailRepository;
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionEmail = _configuration.GetValue<string>("SubscriptionName");
            orderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);
            orderUpadatePaymentStatusProcessor = client.CreateProcessor(orderUpdatePaymentResultTopic, subscriptionEmail);
            _emailRepository = emailRepository;
        }

        public async Task Start()
        {
            orderUpadatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceivedAsync;
            orderUpadatePaymentStatusProcessor.ProcessErrorAsync += ErrorHeandler;
            await orderUpadatePaymentStatusProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await orderUpadatePaymentStatusProcessor.StopProcessingAsync();
            await orderUpadatePaymentStatusProcessor.DisposeAsync();
        }

        Task ErrorHeandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());

            return Task.CompletedTask;
        }

        private async Task OnOrderPaymentUpdateReceivedAsync(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var updatePaymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            try
            {
                await _emailRepository.SendAndLogEmail(updatePaymentResultMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
