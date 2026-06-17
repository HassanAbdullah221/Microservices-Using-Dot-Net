//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace WebApplication4.Security
//{
//    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
//    {
//        public async Task OnActionExecutionAsync(
//            ActionExecutingContext context,
//            ActionExecutionDelegate next)
//        {
//            var config = context.HttpContext.RequestServices
//                .GetRequiredService<IConfiguration>();

//            if (!context.HttpContext.Request.Headers
//                .TryGetValue("X-API-KEY", out var apiKey))
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }

//            if (apiKey != config["ApiKey"])
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }

//            await next();
//        }
//    }
//}
