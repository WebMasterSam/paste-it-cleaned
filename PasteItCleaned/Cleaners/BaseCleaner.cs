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

        public virtual string Clean(string content, Config config)
        {
            return content;
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

        protected HtmlDocument SafeExec(Func<HtmlDocument, HtmlDocument> act, HtmlDocument content)
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

        protected HtmlDocument SafeExec(Func<string, HtmlDocument> act, string content)
        {
            try
            {
                return act.Invoke(content);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError(ex);
                return null;
            }
        }
    }
}
