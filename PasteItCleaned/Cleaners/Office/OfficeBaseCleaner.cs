using PasteItCleaned.Entities;

namespace PasteItCleaned.Cleaners.Office
{
    public class OfficeBaseCleaner : HtmlCleaner
    {
        public override string Clean(string content, Config config)
        {
            var cleaned = content;
            
            cleaned = base.SafeExec(base.ParseWithHtmlAgilityPack, cleaned);
            cleaned = base.SafeExec(base.AddInlineStyles, cleaned);
            cleaned = base.SafeExec(base.ConvertBulletLists, cleaned);
            cleaned = base.SafeExec(base.AddInlineStyles, cleaned); // To ensure new UL will receive styles
            cleaned = base.SafeExec(this.RemoveUselessAttributes, cleaned);
            cleaned = base.SafeExec(this.RemoveClassAttributes, cleaned);
            cleaned = base.SafeExec(this.RemoveUselessStyles, cleaned);
            
            cleaned = base.Clean(cleaned, config);

            return cleaned;
        }

        // Gérer les versions plus/moins récentes de Office
    }
}
