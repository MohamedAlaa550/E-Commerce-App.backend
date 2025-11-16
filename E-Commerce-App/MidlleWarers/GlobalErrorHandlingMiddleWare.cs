using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Shared.ErrorModels;
using System.Net;

namespace E_Commerce_App.MidlleWarers
{
    public class GlobalErrorHandlingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleWare> _logger;

        public GlobalErrorHandlingMiddleWare(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleWare> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await HandleNotFoundEndPointAsync(httpContext);

            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong: {exception}");
                await HndleExceptionAsync(httpContext, exception);

            }

        }

        private async Task HndleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException  => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };
            var response = new ErrorDetails
           {
                StatusCode = httpContext.Response.StatusCode,
                ErrorMessage = exception.Message
           }.ToString();
            await httpContext.Response.WriteAsync(response);
        }
        private async Task HandleNotFoundEndPointAsync (HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                ErrorMessage = $" The End Point {httpContext.Request.Path} Not Found"
            }.ToString(); 
            await httpContext.Response.WriteAsync(response);
        }
    }
}
