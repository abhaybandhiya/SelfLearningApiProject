using System.Diagnostics;
using System.Text;

namespace SelfLearningApiProject.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context) //ye middleware pipeline me har request ke liye call hota hai
        {
            // 1. Time measure karne ke liye stopwatch
            var watch = Stopwatch.StartNew();

            // 2. Read request details
            var requestInfo = await FormatRequest(context);

            // 3. Middleware pipeline me next middleware ko call
            await _next(context);

            // 4. Response complete hone ke baad duration calculate
            watch.Stop();

            // 5. Response details collect
            var responseInfo = FormatResponse(context, watch.ElapsedMilliseconds);

            // 6. Console me print
            Console.ForegroundColor = ConsoleColor.DarkRed; // ye console text color set karta hai
            Console.WriteLine("----- REQUEST -----");
            Console.WriteLine(requestInfo);
            Console.WriteLine("----- RESPONSE -----");
            Console.WriteLine(responseInfo);
            Console.ResetColor();
        }

        private async Task<string> FormatRequest(HttpContext context)
        {
            context.Request.EnableBuffering(); // request body multiple times read karne ke liye

            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength ?? 0)];
            await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
            string body = Encoding.UTF8.GetString(buffer);

            context.Request.Body.Position = 0; // reset stream

            return $"Method: {context.Request.Method}\n" +
                   $"Path: {context.Request.Path}\n" +
                   $"Body: {body}\n";
        }

        private string FormatResponse(HttpContext context, long timeTaken)
        {
            return $"Status Code: {context.Response.StatusCode}\n" +
                   $"Time Taken: {timeTaken} ms\n";
        }
    }
}
