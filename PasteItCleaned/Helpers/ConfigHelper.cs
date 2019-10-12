using Microsoft.Extensions.Configuration;

namespace PasteItCleaned.Helpers
{
    public static class ConfigHelper
    {
        private static IConfiguration _config;

        internal static void SetConfigurationInstance(IConfiguration config)
        {
            _config = config;
        }

        public static string GetAppSetting(string jsonPath)
        {
            return _config.GetValue<string>(jsonPath.Replace(".", ":"));
        }

        public static decimal GetAppSettingDecimal(string jsonPath)
        {
            return _config.GetValue<decimal>(jsonPath.Replace(".", ":"));
        }
    }
}
