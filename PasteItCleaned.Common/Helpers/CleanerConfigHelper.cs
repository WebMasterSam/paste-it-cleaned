using Microsoft.AspNetCore.Http;

namespace PasteItCleaned.Plugin.Helpers
{
    public static class CleanerConfigHelper
    {
        public static string GetConfigFromHeaders(HttpContext context)
        {
            var apiKey = context.Request.Headers["Config"];

            return apiKey.Count > 0 ? apiKey[0] : "";
        }
    }
}
