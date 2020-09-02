using PasteItCleaned.Core.Entities;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners.LibreOffice.All
{
    public class LibreOfficeAllCleaner : LibreOfficeBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.LibreOffice;
        }

        public override bool CanClean(string html, string rtf)
        {
            return html.ToLower().Contains("<meta name=\"Generator\" content=\"LibreOffice".ToLower());
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            return base.Clean(html, rtf, config, keepStyles);
        }
    }
}
