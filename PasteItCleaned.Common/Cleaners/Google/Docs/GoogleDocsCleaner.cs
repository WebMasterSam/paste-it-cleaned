using PasteItCleaned.Core.Entities;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners.Google.Docs
{
    public class GoogleDocsCleaner : GoogleBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.GoogleDocs;
        }

        public override bool CanClean(string html, string rtf)
        {
            return html.ToLower().Contains("\"docs-internal-guid-");
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            return base.Clean(html, rtf, config, keepStyles);
        }
    }
}
