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

        public override string Clean(string content)
        {
            var cleaned = content;

            cleaned = base.SafeExec(base.ParseWithHtmlAgilityPack, cleaned);

            cleaned = base.SafeExec(base.AddInlineStyles, cleaned);

            cleaned = base.SafeExec(this.RemoveUselessAttributes, cleaned);
            cleaned = base.SafeExec(this.RemoveUselessStyles, cleaned);
            cleaned = base.SafeExec(base.RemoveUselessTags, cleaned);

            cleaned = base.SafeExec(base.RemoveImageVTags, cleaned);

            cleaned = base.Clean(cleaned);

            return cleaned;
        }
    }
}
