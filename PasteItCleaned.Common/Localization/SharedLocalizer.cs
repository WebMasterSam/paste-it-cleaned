using Microsoft.Extensions.Localization;

using System.Reflection;

namespace PasteItCleaned.Plugin.Localization
{
    public static class T
    {
        private static SharedLocalizer _loc = null;
        private static IStringLocalizerFactory _factory = null;

        public static void SetStringLocalizerFactory(IStringLocalizerFactory factory)
        {
            _factory = factory;
            _loc = new SharedLocalizer(_factory);
        }

        public static string Get(string key)
        {
            return _loc[key].Value;
        }
    }

    public class SharedLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public SharedLocalizer(IStringLocalizerFactory factory)
        {
            var type = typeof(Resources.SharedResources);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);

            _localizer = factory.Create("Localization.Resources.SharedResources", assemblyName.Name);
        }

        public LocalizedString this[string key] => _localizer.GetString(key);
    }
}
