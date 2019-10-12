using Microsoft.AspNetCore.Http;
using PasteItCleaned.Entities;
using System;

namespace PasteItCleaned.Helpers
{
    public static class CleanerConfigHelper
    {
        public static string GetConfigFromHeaders(HttpContext context)
        {
            var apiKey = context.Request.Headers["Config"];

            return apiKey.Count > 0 ? apiKey[0] : "";
        }

        public static Config GetConfigFromDb(Guid clientId, string config)
        {
            var client = DbHelper.SelectClient(clientId);
            var configObj = client.Configs.Find((c) => { return c.Name.ToLower().Trim() == config.ToLower().Trim(); });
            var configDefaultObj = client.Configs.Find((c) => { return c.Name.ToLower().Trim() == "default"; });

            return configObj != null ? configObj : configDefaultObj;
        }
    }
}
