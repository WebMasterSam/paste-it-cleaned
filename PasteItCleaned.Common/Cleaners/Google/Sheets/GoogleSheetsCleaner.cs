using PasteItCleaned.Core.Entities;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners.Google.Sheets
{
    public class GoogleSheetsCleaner : GoogleBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.GoogleSheets;
        }

        public override bool CanClean(string html, string rtf)
        {
            return html.ToLower().Contains("<google-sheets-html-origin>");
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            return base.Clean(html.Replace("<google-sheets-html-origin>", ""), rtf, config, keepStyles);
        }
    }
}
