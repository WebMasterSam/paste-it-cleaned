using PasteItCleaned.Entities;

namespace PasteItCleaned.Cleaners.Web
{
    public class WebCleaner : HtmlCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.Web;
        }

        public override bool CanClean(string content)
        {
            return true;
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            var cleaned = html;

            if (config.GetConfigValue("removeIframes", true))
                cleaned = base.SafeExec(base.RemoveIframes, cleaned);

            var htmlDoc = base.ParseWithHtmlAgilityPack(cleaned);
            var rtfDoc = base.ParseWithRtfPipe(rtf);

            if (config.GetConfigValue("embedExternalImages", false))
                htmlDoc = base.SafeExec(this.EmbedExternalImages, htmlDoc, rtfDoc);

            if (config.GetConfigValue("removeTagAttributes", true))
                htmlDoc = base.SafeExec(this.RemoveUselessAttributes, htmlDoc, rtfDoc);

            if (config.GetConfigValue("removeClassNames", true))
            {
                htmlDoc = base.SafeExec(base.AddInlineStyles, htmlDoc, rtfDoc);
                htmlDoc = base.SafeExec(this.RemoveClassAttributes, htmlDoc, rtfDoc);
            }

            return base.Clean(base.GetOuterHtml(htmlDoc), rtf, config, keepStyles);
        }
    }
}
