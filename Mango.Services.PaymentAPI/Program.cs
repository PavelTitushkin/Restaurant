using Mango.Services.PaymentAPI.Extensions;
using Mango.Services.PaymentAPI.MessageBus;
using Mango.Services.PaymentAPI.Messaging;
using PaymentProcessor;

namespace Mango.Services.PaymentAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //Add dependencies
            builder.Services.AddSingleton<IProcessorPayment, ProcessorPayment>();
            builder.Services.AddSingleton<IAzureServiceBusConsumerPayment, AzureServiceBusConsumerPayment>();
            builder.Services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseAzureServiceBusConsumer();

            app.Run();
        }
    }
}