using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasteItCleaned.Cleaners.Office
{
    public class OfficeBaseCleaner : HtmlCleaner
    {
        public override string Clean(string content)
        {
            var cleaned = content;

            cleaned = this.RemoveTitle(cleaned);
            cleaned = this.RemoveClassnames(cleaned);
            cleaned = this.RemoveUselessTags(cleaned);
            cleaned = this.RemoveEmptyParagraphs(cleaned);
            cleaned = this.RemoveImageVTags(cleaned);
            cleaned = this.RemoveExtraLines(cleaned);
            cleaned = this.FixEntities(cleaned);

            cleaned = base.Clean(content);

            return cleaned;
        }

        private string RemoveTitle(string content)
        {
            var Pattern = @"<title>(w|W)+?</title>";

            return Regex.Replace(content, Pattern, "");
        }

        private string RemoveClassnames(string content)
        {
            var Pattern = @"s?class=w+";

            return Regex.Replace(content, Pattern, "");
        }

        private string RemoveUselessTags(string content)
        {
            var Pattern = @"<(meta|link|/?o:|/?style|/?div|/?std|/?head|/?html|body|/?body|/?span|![)[^>]*?>";

            return Regex.Replace(content, Pattern, "");
        }

        private string RemoveEmptyParagraphs(string content)
        {
            var Pattern = @"(<[^>]+>)+&nbsp;(</w+>)+";

            return Regex.Replace(content, Pattern, "");
        }

        private string RemoveImageVTags(string content)
        {
            var Pattern = @"s+v:w+=""[^""]+""";

            return Regex.Replace(content, Pattern, "");
        }

        private string RemoveExtraLines(string content)
        {
            var Pattern = @"(\n\r){2,}";

            return Regex.Replace(content, Pattern, "");
        }

        private string FixEntities(string content)
        {
            NameValueCollection nvc = new NameValueCollection();

            nvc.Add("\"", "&ldquo;");
            nvc.Add("\"", "&rdquo;");
            nvc.Add("Ã¢â‚¬â€œ", "&mdash;");

            foreach (string key in nvc.Keys)
            {
                content = content.Replace(key, nvc[key]);
            }

            return content;
        }
    }
}
