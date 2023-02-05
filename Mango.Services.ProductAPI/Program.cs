using AutoMapper;
using Mango.Services.ProductAPI.DataBase;
using Mango.Services.ProductAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI
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

            //Add AutoMapper
            IMapper mapper = MappingConfig.RegisterMap().CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Add dependensies
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

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

            app.Run();
        }
    }
}