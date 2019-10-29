using System;

namespace PasteItCleaned.Common.Helpers
{
    public static class Base64Helper
    {
        public static byte[] GetBytes(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        public static string GetBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}
