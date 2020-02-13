using PasteItCleaned.Core.Entities;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners.Office.PowerPoint
{
    public class OfficePowerPointCleaner : OfficeBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.PowerPoint;
        }

        public override bool CanClean(string html, string rtf)
        {
            return html.ToLower().Contains("<meta name=Generator content=\"Microsoft Power".ToLower());
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            return base.Clean(html, rtf, config, keepStyles);
        }
    }
}
