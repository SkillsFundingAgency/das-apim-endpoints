namespace SFA.DAS.Apprenticeships.Api.Middleware
{
    public class LogRequestMiddleware
    {

        private ILogger<LogRequestMiddleware> _logger;
        private readonly RequestDelegate _next;

        public LogRequestMiddleware(RequestDelegate next, ILogger<LogRequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Middleware Request Path: {context.Request.Path} Headers:{HeadersToString(context)}");
            await _next(context);
        }

        private string HeadersToString(HttpContext context)
        {
            var headers = context.Request.Headers;

            var headersString = "";

            foreach (var header in headers)
            {
                headersString += $"{header.Key}: {string.Join(", ", header.Value)} ";
            }

            return headersString;
        }
    }
}
