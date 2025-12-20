using System.Net;
using System.Text.Json;

namespace SelfLearningApiProject.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

       public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger)
       {
            _next = next;
            _logger = logger;
       }

        public async Task InvokeAsync(HttpContext context)// jo har request ke liye call hota he
        {
            try
            {
                await _next(context); // next middleware / controller
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Something went wrong. Please try again later."
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
