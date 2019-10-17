using HtmlAgilityPack;

using PasteItCleaned.Entities;
using PasteItCleaned.Helpers;

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

        public virtual string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            return html;
        }

        protected string SafeExec(Func<string, string> act, string content)
        {
            try
            {
                return act.Invoke(content);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError(ex);
                return content;
            }
        }

        protected HtmlDocument SafeExec(Func<HtmlDocs, HtmlDocument> act, HtmlDocument html, HtmlDocument rtf)
        {
            try
            {
                return act.Invoke(new HtmlDocs(html, rtf));
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError(ex);
                return html;
            }
        }
    }
}
