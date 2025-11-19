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
            var response = new ErrorDetails
            {
                ErrorMessage = exception.Message
            };
            // httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnAuthorizedException => StatusCodes.Status401Unauthorized,
                ValidationsExceptions validationsExceptions =>
                HandleValidationExceptions(validationsExceptions, response),
                _ => (int)HttpStatusCode.InternalServerError
            };
            response.StatusCode = httpContext.Response.StatusCode;

            await httpContext.Response.WriteAsync(response.ToString());
        }
        private async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                ErrorMessage = $" The End Point {httpContext.Request.Path} Not Found"
            }.ToString();
            await httpContext.Response.WriteAsync(response);
        }

        private int HandleValidationExceptions(ValidationsExceptions validationsExceptions, ErrorDetails response)
        {
            response.Errors = validationsExceptions.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
