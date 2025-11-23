using ECommerceG02.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Presentation.Attributes
{
    public class RedisCacheAttribute(int durationInSeconds = 120) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var servicesManager = context.HttpContext.RequestServices.GetService(typeof(IServicesManager)) as IServicesManager;
            var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<RedisCacheAttribute>)) as ILogger;
            var cacheService = servicesManager.CacheServices;

            string cacheKey = GenerateCacheKey(context);
            var cachedData = await cacheService.GetAsync<object>(cacheKey);

            if (cachedData is not null)
            {
                context.Result = new ContentResult
                {
                    Content = System.Text.Json.JsonSerializer.Serialize(cachedData),
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is ObjectResult objectResult)
            {

                if (objectResult.StatusCode == 200)
                {
                    var value = objectResult.Value;
                    if (value is not null)
                    {
                        await cacheService.SetAsync(cacheKey, value, TimeSpan.FromSeconds(durationInSeconds));
                    }
                } 
            }
        }

        private string GenerateCacheKey(ActionExecutingContext context)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(context.HttpContext.Request.Path.ToString());
            foreach (var (key, value) in context.ActionArguments.OrderBy(k => k.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
