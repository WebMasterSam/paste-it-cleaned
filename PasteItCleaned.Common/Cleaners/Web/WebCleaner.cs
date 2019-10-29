using PasteItCleaned.Common.Entities;

namespace PasteItCleaned.Common.Cleaners.Web
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

            if (config.GetConfigValue("RemoveIframes", true))
                cleaned = base.SafeExec(base.RemoveIframes, cleaned);

            var htmlDoc = base.ParseWithHtmlAgilityPack(cleaned);
            var rtfDoc = base.ParseWithRtfPipe(rtf);

            if (config.GetConfigValue("EmbedExternalImages", false))
                htmlDoc = base.SafeExec(this.EmbedExternalImages, htmlDoc, rtfDoc);

            if (config.GetConfigValue("RemoveTagAttributes", true))
                htmlDoc = base.SafeExec(this.RemoveUselessAttributes, htmlDoc, rtfDoc);

            if (config.GetConfigValue("RemoveClassNames", true))
            {
                htmlDoc = base.SafeExec(base.AddInlineStyles, htmlDoc, rtfDoc);
                htmlDoc = base.SafeExec(this.RemoveClassAttributes, htmlDoc, rtfDoc);
            }

            return base.Clean(base.GetOuterHtml(htmlDoc), rtf, config, keepStyles);
        }
    }
}
