using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners.OpenOffice
{
    public class OpenOfficeBaseCleaner : HtmlCleaner
    {
        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            var cleaned = base.SafeExec(base.RemoveVmlComments, html);

            var htmlDoc = base.ParseWithHtmlAgilityPack(cleaned);
            var rtfDoc = base.ParseWithRtfPipe(rtf);

            htmlDoc = base.SafeExec(base.AddInlineStyles, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.ConvertFontHeadersForOpenOffice, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.ConvertAttributesToStyles, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.ConvertAttributesSizeToStyles, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.AddDefaultOpenOfficeStyles, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.UnifyHeaders, htmlDoc, rtfDoc, config);
            //htmlDoc = base.SafeExec(base.RemoveUselessNestedTextNodes, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.RemoveClassAttributes, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(base.RemoveUselessStyles, htmlDoc, rtfDoc, config);

            if (config.EmbedExternalImages)
                htmlDoc = base.SafeExec(base.EmbedExternalImages, htmlDoc, rtfDoc, config);

            cleaned = base.GetOuterHtml(htmlDoc);

            return base.Clean(cleaned, rtf, config, keepStyles);
        }
    }
}
