using PasteItCleaned.Core.Entities;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners.Web
{
    public class WebCleaner : HtmlCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.Web;
        }

        public override bool CanClean(string html, string rtf)
        {
            return !string.IsNullOrWhiteSpace(html);
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            var cleaned = html;

            if (config.RemoveIframes)
                cleaned = base.SafeExec(base.RemoveIframes, cleaned);

            var htmlDoc = base.ParseWithHtmlAgilityPack(cleaned);
            var rtfDoc = base.ParseWithRtfPipe(rtf);

            if (config.EmbedExternalImages)
                htmlDoc = base.SafeExec(this.EmbedExternalImages, htmlDoc, rtfDoc);

            if (config.RemoveTagAttributes)
                htmlDoc = base.SafeExec(this.RemoveUselessAttributes, htmlDoc, rtfDoc);

            if (config.RemoveClassNames)
            {
                htmlDoc = base.SafeExec(base.AddInlineStyles, htmlDoc, rtfDoc);
                htmlDoc = base.SafeExec(this.RemoveClassAttributes, htmlDoc, rtfDoc);
            }

            htmlDoc = base.SafeExec(base.RemoveEmptyAttributes, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.RemoveUselessTags, htmlDoc, rtfDoc);

            return base.Clean(base.GetOuterHtml(htmlDoc), rtf, config, keepStyles);
        }
    }
}
