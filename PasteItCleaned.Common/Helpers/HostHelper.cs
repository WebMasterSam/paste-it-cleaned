using Microsoft.AspNetCore.Http;
using System;

namespace PasteItCleaned.Common.Helpers
{
    public static class HostHelper
    {
        public static string GetHostFromHeaders(HttpContext context)
        {
            var origin = context.Request.Headers["Origin"];
            var referer = context.Request.Headers["Referer"];

            if (!string.IsNullOrWhiteSpace(origin))
                return new Uri(origin).Host.Split(':')[0].ToLower().Trim().Replace("www.", "");

            if (!string.IsNullOrWhiteSpace(referer))
                return new Uri(referer).Host.Split(':')[0].ToLower().Trim().Replace("www.", "");

            return "";
        }
    }
}
