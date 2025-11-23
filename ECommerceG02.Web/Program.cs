using ECommerceG02.Abstractions.Services;
using ECommerceG02.Configuration;
using ECommerceG02.Domian.Contacts;
using ECommerceG02.Domian.Contacts.Repos;
using ECommerceG02.Domian.Contacts.UOW;
using ECommerceG02.Domian.Models.Identity;
using ECommerceG02.Presentation.Attributes;
using ECommerceG02.Presentation.Controllers;
using ECommerceG02.Presistence.Contexts;
using ECommerceG02.Presistence.Helpers;
using ECommerceG02.Presistence.Repos;
using ECommerceG02.Presistence.UOW;
using ECommerceG02.Services.MappingProfiles;
using ECommerceG02.Services.Services;
using ECommerceG02.Shared.ErrorModels;
using ECommerceG02.Web.CustomMiddlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ECommerceG02.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDatabaseHelper, DatabaseHelper>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServicesManager, ServicesManager>();


            builder.Services.AddAuthenticationServices(builder.Configuration);

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new Shared.ErrorModels.ValidationError
                        {
                            Field = e.Key,
                            Errors = e.Value.Errors.Select(er => er.ErrorMessage)
                        });
                    var Response = new ValidationErrorToReturn()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(Response);
                };
            });

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<RedisCacheAttribute>();

            builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(
                    builder.Configuration.GetConnectionString("RedisConnection"),
                    true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddAutoMapper(p => p.AddProfile(new ProjectProfile()));
            builder.Services.AddTransient<ProductUrlResolver>();
            builder.Services.AddTransient<OrderProductUrlResolver>();

            builder.Services.AddControllers().AddApplicationPart(typeof(AuthController).Assembly);
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });

                options.AddPolicy("Production", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });


            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var dbHelper = services.GetRequiredService<IDatabaseHelper>();
                await dbHelper.InitializeAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during database initialization");

            }
            app.UseMiddleware<CustomExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "Production");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        
    }
}