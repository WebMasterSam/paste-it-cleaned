using PasteItCleaned.Entities;

namespace PasteItCleaned.Cleaners.Office
{
    public class OfficeBaseCleaner : HtmlCleaner
    {
        public override string Clean(string html, string rtf, Config config)
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
            htmlDoc = base.SafeExec(this.RemoveUselessAttributes, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(this.RemoveClassAttributes, htmlDoc, rtfDoc);
            htmlDoc = base.SafeExec(this.RemoveUselessStyles, htmlDoc, rtfDoc);

            cleaned = base.GetOuterHtml(htmlDoc);

            return base.Clean(cleaned, rtf, config);
        }
    }
}


//return 0 <= t.indexOf("\\pngblip") ? Fo.value("image/png") : 0 <= t.indexOf("\\jpegblip") ? Fo.value("image/jpeg") : Fo.error("errors.imageimport.unsupported")