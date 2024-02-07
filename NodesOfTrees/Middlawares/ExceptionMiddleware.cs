using NodesOfTrees.Models;
using System.Net;
using System.Text.Json;
using NodesOfTrees.Abstractions.Interfaces;
using NodesOfTrees.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace NodesOfTrees.Middlawares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                try
                {
                    await _next(httpContext);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong: {ex}");
                    await HandleExceptionAsync(httpContext, ex, context);
                }
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, DataContext dataContext)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var exceptionType = exception.GetType().Name;
            var eventId = Guid.NewGuid().ToString();

            var errorDetails = new ExceptionLog
            {
                EventId = eventId,
                Timestamp = DateTime.UtcNow,
                QueryParameters = JsonSerializer.Serialize(context.Request.Query),
                BodyParameters = "",
                StackTrace = exception.StackTrace
            };

            dataContext.ExceptionLogs.Add(errorDetails);
            dataContext.SaveChanges();

            if (exception is SecureException)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync(new ErrorDetails
                {
                    Type = "Secure",
                    Id = eventId,
                    Data = new { exception.Message }
                }.ToString());
            }

            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(new ErrorDetails
            {
                Type = exceptionType,
                Id = eventId,
                Data = new { Message = $"Internal server error ID = {eventId}" }
            }.ToString());
        }
    }
}
