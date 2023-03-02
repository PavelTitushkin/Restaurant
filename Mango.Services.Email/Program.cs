using Mango.Services.Email.DataBase;
using Mango.Services.Email.Extensions;
using Mango.Services.Email.Messaging;
using Mango.Services.Email.Repository;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Email
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

            //Add Database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(connectionString));
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer(connectionString);

            builder.Services.AddScoped<IEmailRepository, EmailRepository>();
            builder.Services.AddSingleton(new EmailRepository(optionBuilder.Options));
            //builder.Services.AddSingleton <IAzureServiceBusConsumerEmail, AzureServiceBusConsumerEmail>();
            builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

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

            //app.UseAzureServiceBusConsumer();

            app.Run();
        }
    }
}