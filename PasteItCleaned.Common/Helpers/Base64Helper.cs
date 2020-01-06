using System;
using System.Text;

namespace PasteItCleaned.Common.Helpers
{
    public static class Base64Helper
    {
        public static byte[] GetBytes(string base64)
        {
            var s = base64.Replace('-', '+').Replace('_', '/').PadRight(4 * ((base64.Length + 3) / 4), '=');

            return Convert.FromBase64String(s);
        }

        public static string GetString(string base64)
        {
            return Encoding.ASCII.GetString(Base64Helper.GetBytes(base64));
        }

        public static string GetBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}
