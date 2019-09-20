using PasteItCleaned.Cleaners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Helpers
{
    public static class ErrorHelper
    {
        private const string ProjectPrefix = "PasteItCleaned";
        private const string ErrorFormat = "[{0} ERR-{1}]";
        private const string MessageFormat = "{0}: {1}";

        public static string GetApiKeyDomainNotConfigured()
        {
            return Error(12, "The domain where the plugin was used (XXX) is not configure for the account with API key (YYY).");
        }

        public static string GetApiKeyInvalid()
        {
            return Error(11, "Your API key is invalid (YYY).");
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
            return string.Format(MessageFormat, Code(number), message);
        }
    }
}
