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

            var htmlDoc = base.ParseWithHtmlAgilityPack(cleaned);
            var rtfDoc = base.ParseWithRtfPipe(rtf);

            htmlDoc = base.SafeExec(base.ConvertFontHeaders, htmlDoc, rtfDoc, config);

            if (config.EmbedExternalImages)
                htmlDoc = base.SafeExec(this.EmbedExternalImages, htmlDoc, rtfDoc, config);

            if (config.RemoveTagAttributes)
                htmlDoc = base.SafeExec(this.RemoveUselessAttributes, htmlDoc, rtfDoc, config);

            if (config.RemoveClassNames)
            {
                htmlDoc = base.SafeExec(base.AddInlineStyles, htmlDoc, rtfDoc, config);
                htmlDoc = base.SafeExec(this.RemoveClassAttributes, htmlDoc, rtfDoc, config);
            }

            htmlDoc = base.SafeExec(base.UnifyHeaders, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.ConvertAttributesToStyles, htmlDoc, rtfDoc, config);

            return base.Clean(base.GetOuterHtml(htmlDoc), rtf, config, keepStyles);
        }
    }
}
