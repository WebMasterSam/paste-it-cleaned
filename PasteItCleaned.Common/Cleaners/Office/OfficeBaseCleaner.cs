using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners.Office
{
    public class OfficeBaseCleaner : HtmlCleaner
    {
        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            var cleaned = base.SafeExec(base.RemoveVmlComments, html);

            var htmlDoc = base.ParseWithHtmlAgilityPack(cleaned);
            var rtfDoc = base.ParseRtf(rtf);

            htmlDoc = base.SafeExec(base.AddInlineStyles, htmlDoc, rtfDoc, config); // For lists, it's necessary to do a first pass to add inline styles before converting to LI
            htmlDoc = base.SafeExec(base.AddVShapesTags, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.ConvertFontHeaders, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.ConvertBulletLists, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.AddInlineStyles, htmlDoc, rtfDoc, config); // To ensure new UL/OL will receive styles
            htmlDoc = base.SafeExec(base.ConvertAttributesToStyles, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.RemoveClassAttributes, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.RemoveUselessStyles, htmlDoc, rtfDoc, config);

            if (config.EmbedExternalImages)
                htmlDoc = base.SafeExec(base.EmbedExternalImages, htmlDoc, rtfDoc, config);

            cleaned = base.GetOuterHtml(htmlDoc);

            return base.Clean(cleaned, rtf, config, keepStyles);
        }
    }
}
