using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace PasteItCleaned.Common.Helpers
{
    public static class ImageHelper
    {
        //private static readonly HttpClient _httpClient = new HttpClient();

        public static string GetExternalDataUri(string url, string width, string height)
        {
            try
            {
                using (var webClient = new WebClientWithTimeout())
                {
                    var bytes = webClient.DownloadData(url);
                    var pngOriginal = ConvertToPng(bytes);
                    var pngResized = (Image)null;

                    if (!string.IsNullOrWhiteSpace(width) && !string.IsNullOrWhiteSpace(height))
                    {
                        var widthPx = int.Parse(width.Replace("px", ""));
                        var heightPx = int.Parse(height.Replace("px", ""));

                        pngResized = ResizeImage(pngOriginal, widthPx, heightPx);
                    }

                    var pngBase64 = GetBase64(pngResized != null ? pngResized : pngOriginal);

                    return string.Format("data:image/png;base64,{0}", pngBase64);
                }
            }
            catch
            {
                return url;
            }
        }

        public static string ConvertToPngDataUri(string dataUri, string width, string height)
        {
            try
            {
                if (dataUri.ToLower().StartsWith("data:windows/metafile;base64"))
                {
                    var base64 = dataUri.Substring(29);
                    var converterType = ConfigHelper.GetAppSetting("Images.WindowMetafile.Converter.Type");

                    if (converterType == "External")
                    {
                        var converterEndpoint = ConfigHelper.GetAppSetting("Images.WindowMetafile.Converter.Endpoint");
                        var val = string.Format(@"{{ ""wmfBase64"": ""{0}"", ""width"": {1}, ""height"": {2} }}", base64, width, height);
                        var values = GetBytes(val);

                        using (var webClient = new WebClientWithTimeout())
                        {
                            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                            var response = webClient.UploadData(converterEndpoint, "POST", values);
                            var base64Response = GetString(response);
                            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<MetafileConverterResponse>(base64Response);

                            return string.Format("data:image/png;base64,{0}", json.base64);
                        }


                        /*var values = new Dictionary<string, string>
                        {
                            { "wmfBase64", base64 },
                            { "width", width.ToString() },
                            { "height", height.ToString() }
                        };

                        var content = new FormUrlEncodedContent(values);
                        var response = _httpClient.PostAsync(converterEndpoint, content);
                        
                        response.Wait(6000); // Max 6 secondes d'attente

                        if (response.IsCompleted)
                        {
                            var bytes = response.Result.Content.ReadAsByteArrayAsync();

                            if (bytes.IsCompleted)
                            {
                                var pngBase64 = GetBase64(bytes.Result);

                                return string.Format("data:image/png;base64,{0}", pngBase64);
                            }
                        }*/
                    }
                    else
                    {
                        var bytes = Convert.FromBase64String(base64);
                        var metafileOriginal = ConvertToMetafile(bytes);
                        var pngOriginal = ConvertToPng(GetBytes(metafileOriginal));
                        var pngResized = (Image)null;

                        if (!string.IsNullOrWhiteSpace(width) && !string.IsNullOrWhiteSpace(height))
                        {
                            var widthPx = int.Parse(width.Replace("px", ""));
                            var heightPx = int.Parse(height.Replace("px", ""));

                            pngResized = ResizeImage(pngOriginal, widthPx, heightPx);
                        }

                        var pngBase64 = GetBase64(pngResized != null ? pngResized : pngOriginal);

                        return string.Format("data:image/png;base64,{0}", pngBase64);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError(ex);
            }

            return dataUri;
        }

        public static byte[] GetBytes(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);

                return ms.ToArray();
            }
        }

        public static byte[] GetBytes(String str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string GetBase64(Image img)
        {
            return GetBase64(GetBytes(img));
        }

        public static string GetBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static string GetString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static Stream GetStream(Image img)
        {
            using (var saveMs = new MemoryStream())
            {
                img.Save(saveMs, ImageFormat.Png);

                return saveMs;
            }
        }

        public static Image ConvertToPng(byte[] bytes)
        {
            using (var saveMs = new MemoryStream())
            {
                using (var readMs = new MemoryStream(bytes))
                {
                    var img = Image.FromStream(readMs);

                    img.Save(saveMs, ImageFormat.Png);

                    return Image.FromStream(saveMs);
                }
            }
        }

        public static Image ConvertToMetafile(byte[] bytes)
        {
            using (var readMs = new MemoryStream(bytes))
            {
                return Metafile.FromStream(readMs);
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }

    internal class WebClientWithTimeout : WebClient
    {
        private int _timeout = 2000;

        public WebClientWithTimeout()
        { }

        public WebClientWithTimeout(int timeout)
        {
            this._timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest wr = base.GetWebRequest(address);

            wr.Timeout = this._timeout;

            return wr;
        }
    }

    internal class MetafileConverterResponse
    {
        public string base64 { get; set; }
    }
}
