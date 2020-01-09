using Microsoft.AspNetCore.Http;
using PasteItCleaned.Common.Entities;
using PasteItCleaned.Core.Helpers;
using System;

namespace PasteItCleaned.Common.Helpers
{
    public static class ApiKeyHelper
    {
        public static bool ApiKeyPresent(string apiKey)
        {
            if (!ConfigHelper.GetAppSetting<bool>("Features.ApiKeyValidation"))
                return true;

            return !string.IsNullOrWhiteSpace(apiKey);
        }

        public static bool ApiKeyValid(ApiKey apiKey)
        {
            if (!ConfigHelper.GetAppSetting<bool>("Features.ApiKeyValidation"))
                return true;

            if (apiKey != null)
                if (apiKey.ExpiresOn.Date > DateTime.UtcNow.Date)
                    return true;

            return false;
        }

        public static bool ApiKeyFitsWithDomain(ApiKey apiKey, string domain)
        {
            if (!ConfigHelper.GetAppSetting<bool>("Features.ApiKeyValidation"))
                return true;

            if (domain == "localhost" || domain == "127.0.0.1")
                return true;

            if (apiKey != null)
                if (apiKey.Domains.Contains(domain.Replace("www.", "")))
                    return true;

            return false;
        }

        public static string GetApiKeyFromHeaders(HttpContext context)
        {
            var apiKey = context.Request.Headers["ApiKey"];

            return apiKey.Count > 0 ? apiKey[0] : "";
        }

        public static ApiKey GetApiKeyFromDb(string apiKey)
        {
            return DbHelper.SelectApiKey(apiKey);
        }
    }
}
