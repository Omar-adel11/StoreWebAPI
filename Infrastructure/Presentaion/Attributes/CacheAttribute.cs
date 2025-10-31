using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServicesAbstractions;

namespace Presentaion.Attributes
{
    public class CacheAttribute(int durationInMilliSeconds) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().cacheService;
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var cachedResponse = await cache.GetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                context.Result = new ContentResult()
                {
                    ContentType = "application/json",
                    StatusCode = 200,
                    Content = cachedResponse
                };
                return;
            }
            //Execute the Endpoint
            var executedContext = await next();
            if(executedContext.Result is OkObjectResult okObjectResult)
            {
                await cache.SetCacheValueAsync(cacheKey,okObjectResult.Value, TimeSpan.FromSeconds(durationInMilliSeconds));
            }
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var item in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{item.Key}-{item.Value}");
            }
            return keyBuilder.ToString();
        }
    }
}
