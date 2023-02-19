using Mango.Web.Services;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;

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
            builder.Services.AddHttpClient<ICartService, CartService>();
            builder.Services.AddHttpClient<IProductServices, ProductService>();
            builder.Services.AddHttpClient<ICouponService, CouponService>();

            //AddDependensies
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IProductServices, ProductService>();
            builder.Services.AddScoped<ICouponService, CouponService>();

            //AddInfo
            SD.ProductAPIBase = builder.Configuration["ServiceURLs:ProductAPI"];
            SD.ShoppingCartAPIBase = builder.Configuration["ServiceURLs:ShoppingCartAPI"];
            SD.CouponAPIBase = builder.Configuration["ServiceURLs:CouponAPI"];

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
                    opt.ClaimActions.MapJsonKey("role", "role", "role");
                    opt.ClaimActions.MapJsonKey("sub", "sub", "sub");
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