using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Middleware.Logging
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogDebug("Api.Request.Start", context.Request.Path);

            await _next(context);

            _logger.LogDebug("Api.Request.End", context.Request.Path);
        }
    }
}
