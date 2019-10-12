using PasteItCleaned.Entities;

namespace PasteItCleaned.Cleaners.Office.Excel
{
    public class OfficeExcelCleaner : OfficeBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.Excel;
        }

        public override bool CanClean(string content)
        {
            return content.ToLower().Contains("<meta name=Generator content=\"Microsoft Excel".ToLower());
        }

        public override string Clean(string content, Config config)
        {
            return base.Clean(content, config);
        }
    }
}
