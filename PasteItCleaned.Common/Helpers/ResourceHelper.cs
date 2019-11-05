using System;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace PasteItCleaned.Common.Helpers
{
    public static class ResourceHelper
    {
        public static Image GetEmbeddedImage<T>(string embeddedFileName) where T : class
        {
            var assembly = typeof(T).GetTypeInfo().Assembly;
            var resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(embeddedFileName, StringComparison.CurrentCultureIgnoreCase));

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }

                return Bitmap.FromStream(stream);
            }
        }
    }
}
