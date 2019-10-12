using PasteItCleaned.Entities;

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

        public override string Clean(string content, Config config)
        {
            return base.Clean(content, config);
        }
    }
}
