using Mango.Web.Services;
using Mango.Web.Services.IServices;

namespace Mango.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //AddHttpClient
            builder.Services.AddHttpClient<IProductServices, ProductService>();

            //AddDependensies
            builder.Services.AddScoped<IProductServices, ProductService>();

            //AddInfo
            SD.ProductAPIBase = builder.Configuration["ServiceURLs:ProductAPI"];

            //Add authentication
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "Cookies";
                opt.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(5))
                .AddOpenIdConnect("oidc", opt =>
                {
                    opt.Authority = builder.Configuration["ServiceURLs:IdentityAPI"];
                    opt.GetClaimsFromUserInfoEndpoint = true;
                    opt.ClientId = "mango";
                    opt.ClientSecret = "secret";
                    opt.ResponseType = "code";

                    opt.TokenValidationParameters.NameClaimType = "name";
                    opt.TokenValidationParameters.RoleClaimType = "role";
                    opt.Scope.Add("mango");
                    opt.SaveTokens = true;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}