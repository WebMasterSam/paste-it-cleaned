using HtmlAgilityPack;

namespace PasteItCleaned.Cleaners
{
    public class HtmlDocs
    {
        public HtmlDocs(HtmlDocument html, HtmlDocument rtf)
        {
            this.Html = html;
            this.Rtf = rtf;
        }

        public HtmlDocument Html { get; set; }
        public HtmlDocument Rtf { get; set; }
    }
}
