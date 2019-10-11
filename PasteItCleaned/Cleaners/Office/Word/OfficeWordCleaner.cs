namespace PasteItCleaned.Cleaners.Office.Word
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

        public override string Clean(string content)
        {
            return base.Clean(content);
        }
    }
}
