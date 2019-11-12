using PasteItCleaned.Common.Localization;
using System;

namespace PasteItCleaned.Common.Helpers
{
    public static class ErrorHelper
    {
        private const string ErrorFormat = "[{0} ERR-{1}]";
        private const string MessageFormat = "{0}: {1}";
        private const string MessageTag = "<p style='padding: 5px 10px; font-weight: bold; font-size: 14px; background-color: yellow;'>{0}</p>";

        public static void LogError(Exception ex)
        {
            try
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                DbHelper.InsertError(ex);
            }
            catch { }

            if (ex.InnerException != null)
                LogError(ex.InnerException);
        }

        public static string GetApiKeyDomainNotConfigured(string apiKey, string domain)
        {
            return Error(12, string.Format(T.Get("Error.ApiKey.Domain.NotConfigured"), domain, apiKey));
        }

        public static string GetApiKeyInvalid(string apiKey)
        {
            return Error(11, string.Format(T.Get("Error.ApiKey.Invalid"), apiKey));
        }

        public static string GetApiKeyAbsent()
        {
            return Error(10, T.Get("Error.ApiKey.Missing"));
        }

        public static string GetAccountIsUnpaid()
        {
            return Error(20, T.Get("Error.Billing.PaymentRequired"));
        }

        private static string Code(int number)
        {
            return string.Format(ErrorFormat, ConfigHelper.GetAppSetting("App.Name"), number.ToString().PadLeft(3, '0'));
        }

        private static string Error(int number, string message)
        {
            return string.Format(MessageTag, string.Format(MessageFormat, Code(number), message));
        }
    }
}
