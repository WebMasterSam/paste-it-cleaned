using PasteItCleaned.Core.Entities;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners.OpenOffice.All
{
    public class OpenOfficeAllCleaner : OpenOfficeBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.OpenOffice;
        }

        public override bool CanClean(string html, string rtf)
        {
            return html.ToLower().Contains("<meta name=\"Generator\" content=\"OpenOffice".ToLower());
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            return base.Clean(html, rtf, config, keepStyles);
        }
    }
}
