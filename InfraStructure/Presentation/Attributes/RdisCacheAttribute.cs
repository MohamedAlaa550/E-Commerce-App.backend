using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    public class RedisCacheAttribute(int durationInSecond = 120):ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices
                .GetRequiredService<IServiceManger>().CacheService;
            string cacheKey = GenerateCachKey(context.HttpContext.Request);
            var result = await cacheService.GetCachedValueAsync(cacheKey);
            if(result != null)
            {
                context.Result = new ContentResult
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var contextRsult = await next.Invoke();
            if (contextRsult.Result is OkObjectResult okObject)
                await cacheService.SetCachedValueAsync(cacheKey, okObject.Value!,
                    TimeSpan.FromSeconds(durationInSecond));
        }

        private string GenerateCachKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);
            foreach (var item in request.Query.OrderBy(q=>q.Key))
            {
                keyBuilder.Append($"|{item.Key}-{item.Value}");
            }
            return keyBuilder.ToString();
        }
    }



}
