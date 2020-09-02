using HtmlAgilityPack;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners
{
    public class HtmlDocs
    {
        public HtmlDocs(HtmlDocument html, HtmlDocument rtf, Config config)
        {
            this.Html = html;
            this.Rtf = rtf;
            this.Config = config;
        }

        public HtmlDocument Html { get; set; }
        public HtmlDocument Rtf { get; set; }
        public Config Config { get; set; }
    }
}
