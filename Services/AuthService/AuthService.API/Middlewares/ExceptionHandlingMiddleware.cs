using Newtonsoft.Json;
using System.Data;
using System.Net;

namespace LibraryWebApp.AuthService.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string message;

            switch (ex)
            {
                case DirectoryNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = "The requested resource was not found.";
                    break;

                case ArgumentException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "The request contains invalid data.";
                    break;

                case DuplicateNameException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    message = "The resource already exists.";
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "An error occurred while processing your request.";
                    break;
            }

            context.Response.StatusCode = statusCode;
            var result = JsonConvert.SerializeObject(new
            {
                message,
                details = ex.Message
            });

            return context.Response.WriteAsync(result);
        }
    }
}