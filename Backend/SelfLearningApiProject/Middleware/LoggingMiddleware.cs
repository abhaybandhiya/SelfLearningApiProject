using System.Diagnostics;
using System.Text;

namespace SelfLearningApiProject.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _env = environment;
        }
        
        public async Task InvokeAsync(HttpContext context) //ye middleware pipeline me har request ke liye call hota hai
        {
            // 1. Time measure karne ke liye stopwatch
            var watch = Stopwatch.StartNew();

            // 2. Read request details
            //var requestInfo = await FormatRequest(context); // request details format karne ke liye helper method call 
            _logger.LogInformation("Request {Method} {Path}",context.Request.Method,context.Request.Path);

            // 🔹 FULL REQUEST BODY ONLY IN DEV
            if (_env.IsDevelopment())
            {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(context.Request.Body,Encoding.UTF8,leaveOpen: true);

                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("----- REQUEST BODY (DEV ONLY) -----");
                Console.WriteLine(body);
                Console.ResetColor();
            }
                // 3. Middleware pipeline me next middleware ko call
                await _next(context);

            // 4. Response complete hone ke baad duration calculate
            watch.Stop();

            // 5. Response details collect
            //var responseInfo = FormatResponse(context, watch.ElapsedMilliseconds);
            _logger.LogInformation("Response {StatusCode} in {Elapsed}ms",context.Response.StatusCode,watch.ElapsedMilliseconds);
        }

        //private async Task<string> FormatRequest(HttpContext context)
        //{
        //    context.Request.EnableBuffering(); // request body multiple times read karne ke liye

        //    var buffer = new byte[Convert.ToInt32(context.Request.ContentLength ?? 0)];
        //    await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
        //    string body = Encoding.UTF8.GetString(buffer);

        //    context.Request.Body.Position = 0; // reset stream

        //    return $"Method: {context.Request.Method}\n" +
        //           $"Path: {context.Request.Path}\n" +
        //           $"Body: {body}\n";
        //}

        //private string FormatResponse(HttpContext context, long timeTaken)
        //{
        //    return $"Status Code: {context.Response.StatusCode}\n" +
        //           $"Time Taken: {timeTaken} ms\n";
        //}
    }
}
