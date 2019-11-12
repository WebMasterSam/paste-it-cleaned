using Microsoft.Extensions.Configuration;

namespace PasteItCleaned.Common.Helpers
{
    public static class ConfigHelper
    {
        private static IConfiguration _config;

        public static void SetConfigurationInstance(IConfiguration config)
        {
            _config = config;
        }

        public static T GetAppSetting<T>(string jsonPath)
        {
            return _config.GetValue<T>(jsonPath.Replace(".", ":"));
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
