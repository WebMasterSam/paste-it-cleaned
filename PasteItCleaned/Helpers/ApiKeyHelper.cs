using PasteItCleaned.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Helpers
{
    public static class ApiKeyHelper
    {
        public static bool ApiKeyFitsWithDomain()
        {
            return true;
        }

        public static bool ApiKeyPresent()
        {
            // read api key from headers
            return true;
        }

        public static bool ApiKeyValid()
        {
            // read api key from headers
            return true;
        }

        public static string GetApiKeyFromHeaders()
        {
            //System.Web.HttpContext.Request.Headers["myHeaderKeyName"]
            // read api key from headers
            return "";
        }

        public static ApiKey GetApiKeyFromDb(string apiKey)
        {
            return DbHelper.SelectApiKey(apiKey);
        }
    }
}
