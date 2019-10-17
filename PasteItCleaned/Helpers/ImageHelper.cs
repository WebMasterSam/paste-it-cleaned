using System;

namespace PasteItCleaned.Helpers
{
    public static class ImageHelper
    {
        public static string ConvertToPngDataUri(string dataUri, string width, string height)
        {
            if (dataUri.ToLower().StartsWith("data:windows/metafile;base64"))
            {
                var base64 = dataUri.Substring(29);
                var bytes = Convert.FromBase64String(base64);

                using (var saveMs = new System.IO.MemoryStream())
                {
                    using (var readMs = new System.IO.MemoryStream(bytes))
                    {
                        var img = System.Drawing.Image.FromStream(readMs);

                        /*if (!string.IsNullOrWhiteSpace(width))
                            img.Width = int.Parse();*/

                        img.Save(saveMs, System.Drawing.Imaging.ImageFormat.Png);

                        string pngBase64 = Convert.ToBase64String(saveMs.ToArray());

                        return string.Format("data:image/png;base64,{0}", pngBase64);
                    }
                }
            }

            return dataUri;
        }
    }
}
