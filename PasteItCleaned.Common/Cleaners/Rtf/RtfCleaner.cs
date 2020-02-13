using PasteItCleaned.Core.Entities;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners.Office.Word
{
    public class RtfCleaner : OfficeBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.Word;
        }

        public override bool CanClean(string html, string rtf)
        {
            return string.IsNullOrWhiteSpace(html) && !string.IsNullOrWhiteSpace(rtf);
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            var rtfDoc = base.ParseWithRtfPipe(rtf);
            var htmlDoc = rtfDoc;

            htmlDoc = base.SafeExec(base.ConvertFontHeaders, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.ConvertAttributesToStyles, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.ConvertFontFamilies, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.RemoveClassAttributes, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.RemoveUselessStyles, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.RemoveMarginStylesAttr, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.EmbedInternalImages, htmlDoc, rtfDoc);

            if (config.EmbedExternalImages)
                htmlDoc = base.SafeExec(base.EmbedExternalImages, htmlDoc, rtfDoc);

            htmlDoc = base.SafeExec(base.RemoveUselessAttributes, htmlDoc, rtfDoc);

            var cleaned = base.GetOuterHtml(htmlDoc);

            return base.Clean(cleaned, rtf, config, keepStyles);
        }
    }
}
