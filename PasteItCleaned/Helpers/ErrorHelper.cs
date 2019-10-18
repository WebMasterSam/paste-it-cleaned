using System;

namespace PasteItCleaned.Helpers
{
    public static class ErrorHelper
    {
        private const string ProjectPrefix = "PasteItCleaned";
        private const string ErrorFormat = "[{0} ERR-{1}]";
        private const string MessageFormat = "{0}: {1}";
        private const string MessageTag = "<p style='padding: 5px 10px; font-weight: bold; font-size: 14px; background-color: yellow;'>{0}</p>";

        public static void LogError(Exception ex)
        {
            try
            {
                DbHelper.InsertError(ex);
            }
            catch { }

            if (ex.InnerException != null)
                LogError(ex.InnerException);
        }

        public static string GetApiKeyDomainNotConfigured(string apiKey, string domain)
        {
            return Error(12, string.Format("The domain where the plugin was used ({0}) is not configure for the account with API key ({1}).", domain, apiKey));
        }

        public static string GetApiKeyInvalid(string apiKey)
        {
            return Error(11, string.Format("Your API key is invalid ({0}).", apiKey));
        }

        public static string GetApiKeyAbsent()
        {
            return Error(10, "You forgot to add an API key to your script tag.");
        }

        public static string GetAccountIsUnpaid()
        {
            return Error(20, "Your account does not have sufficient credits, your credit card is expired or not working. Please check your payment informations and try again.");
        }

        private static string Code(int number)
        {
            return string.Format(ErrorFormat, ProjectPrefix, number.ToString().PadLeft(3, '0'));
        }

        private static string Error(int number, string message)
        {
            return string.Format(MessageTag, string.Format(MessageFormat, Code(number), message));
        }
    }
}
