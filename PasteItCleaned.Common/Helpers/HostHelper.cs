using Microsoft.AspNetCore.Http;
using System;

namespace PasteItCleaned.Plugin.Helpers
{
    public static class HostHelper
    {
        public static string GetHostFromHeaders(HttpContext context)
        {
            var origin = context.Request.Headers["Origin"];
            var referer = context.Request.Headers["Referer"];

            if (!string.IsNullOrWhiteSpace(origin))
                if (HostHelper.IsUrl(origin))
                    return new Uri(origin).Host.Split(':')[0].ToLower().Trim().Replace("www.", "");

            if (!string.IsNullOrWhiteSpace(referer))
                if (HostHelper.IsUrl(referer))
                    return new Uri(referer).Host.Split(':')[0].ToLower().Trim().Replace("www.", "");

            return "";
        }

        public static bool IsUrl(string url)
        {
            Uri uriResult;

            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
