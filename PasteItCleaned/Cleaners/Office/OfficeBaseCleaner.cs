using PasteItCleaned.Entities;

namespace PasteItCleaned.Cleaners.Office
{
    public class OfficeBaseCleaner : HtmlCleaner
    {
        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            var cleaned = base.SafeExec(base.RemoveVmlComments, html);

            var htmlDoc = base.ParseWithHtmlAgilityPack(cleaned);
            var rtfDoc = base.ParseWithRtfPipe(rtf);

            htmlDoc = base.SafeExec(base.AddInlineStyles, htmlDoc, rtfDoc); // For lists, it's necessary to do a first pass to add inline styles before converting to LI
            htmlDoc = base.SafeExec(base.AddVShapesTags, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.ConvertFontHeaders, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.ConvertBulletLists, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.AddInlineStyles, htmlDoc, rtfDoc); // To ensure new UL/OL will receive styles
            htmlDoc = base.SafeExec(base.ConvertAttributesToStyles, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.RemoveClassAttributes, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.RemoveUselessStyles, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(base.EmbedInternalImages, htmlDoc, rtfDoc);

            if (config.GetConfigValue("EmbedExternalImages", false))
                htmlDoc = base.SafeExec(base.EmbedExternalImages, htmlDoc, rtfDoc);

            htmlDoc = base.SafeExec(base.RemoveUselessAttributes, htmlDoc, rtfDoc);

            cleaned = base.GetOuterHtml(htmlDoc);

            return base.Clean(cleaned, rtf, config, keepStyles);
        }
    }
}
