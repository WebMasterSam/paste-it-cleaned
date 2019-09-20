namespace PasteItCleaned.Cleaners
{
    public class BaseCleaner
    {
        public virtual SourceType GetSourceType()
        {
            return SourceType.Unknown;
        }

        public virtual bool CanClean(string content)
        {
            return false;
        }

        public virtual string Clean(string original, string content)
        {
            return content;
        }
    }
}
