using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Cleaners.Office.PowerPoint
{
    public class OfficePowerPointCleaner : OfficeBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.PowerPoint;
        }

        public override bool CanClean(string content)
        {
            return content.ToLower().Contains("<meta name=Generator content=\"Microsoft Power".ToLower());
        }

        public override string Clean(string content)
        {
            var cleaned = content;

            cleaned = base.Clean(cleaned);

            return cleaned;
        }
    }
}
