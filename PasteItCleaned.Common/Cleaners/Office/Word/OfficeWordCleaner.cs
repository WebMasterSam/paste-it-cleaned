using PasteItCleaned.Common.Entities;

namespace PasteItCleaned.Common.Cleaners.Office.Word
{
    public class OfficeWordCleaner : OfficeBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.Word;
        }

        public override bool CanClean(string content)
        {
            return content.ToLower().Contains("<meta name=Generator content=\"Microsoft Word".ToLower());
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            return base.Clean(html, rtf, config, keepStyles);
        }
    }
}
