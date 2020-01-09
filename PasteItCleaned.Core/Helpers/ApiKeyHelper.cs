using System;

namespace PasteItCleaned.Core.Helpers
{
    public static class ApiKeyHelper
    {
        public static string GenerateApiKey()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(5, 20);
        }
    }
}
