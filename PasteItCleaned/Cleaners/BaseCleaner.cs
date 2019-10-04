using System;

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

        public virtual string Clean(string content)
        {
            return content;
        }

        protected string SafeExec(Func<string, string> act, string content)
        {
            try
            {
                return act.Invoke(content);
            }
            catch
            {
                return content;
            }
        }
    }
}
