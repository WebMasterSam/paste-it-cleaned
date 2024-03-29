﻿using HtmlAgilityPack;
using PasteItCleaned.Core.Entities;
using PasteItCleaned.Core.Models;

using System;

namespace PasteItCleaned.Plugin.Cleaners
{
    public class BaseCleaner
    {
        public virtual SourceType GetSourceType()
        {
            return SourceType.Other;
        }

        public virtual bool CanClean(string html, string rtf)
        {
            return false;
        }

        public virtual string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            return html.Replace(" name=\"\"", "");
        }

        protected string SafeExec(Func<string, string> act, string content)
        {
            try
            {
                return act.Invoke(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return content;
            }
        }

        protected HtmlDocument SafeExec(Func<HtmlDocs, HtmlDocument> act, HtmlDocument html, HtmlDocument rtf, Config config)
        {
            try
            {
                return act.Invoke(new HtmlDocs(html, rtf, config));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return html;
            }
        }
    }
}
