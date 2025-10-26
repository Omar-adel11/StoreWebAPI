using Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Errors;

namespace Store.Web.Middlewares
{
    public class GlobalErrorHandlingMiddlewares
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddlewares> _logger;

        public GlobalErrorHandlingMiddlewares(RequestDelegate next, ILogger<GlobalErrorHandlingMiddlewares> logger)
        {
            _next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                await HandlingNotFoundErrorAsync(context);
            }
            catch (Exception ex)
            {
                //Log the exception (you can use any logging framework here)
                _logger.LogError(ex, ex.Message);
                await HandlingErrorAsync(context, ex);
            }
        }

        private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            //1.set status code for the response
            //2.set content type for the response
            //3.Response object(Body)
            //4.return Response

            context.Response.ContentType = "application/json";
            var respone = new ErrorDetails()
            {
                ErrorMessage = "Internal Server Error. Please try again later."
            };

            respone.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            context.Response.StatusCode = respone.StatusCode;

            await context.Response.WriteAsJsonAsync(respone);
        }

        private static async Task HandlingNotFoundErrorAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                context.Response.ContentType = "application/json";
                var respone = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End point {context.Request.Path} is not found"
                };
                await context.Response.WriteAsJsonAsync(respone);
            }
        }
    }
}
