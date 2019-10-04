using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PasteItCleaned.Cleaners.Office
{
    public class OfficeBaseCleaner : HtmlCleaner
    {
        public override string Clean(string content)
        {
            var cleaned = content;

            //cleaned = this.RemoveUselessTags(cleaned);
            //cleaned = this.RemoveImageVTags(cleaned);

            cleaned = base.SafeExec(this.RemoveEmptyParagraphs, cleaned);

            cleaned = base.Clean(cleaned);

            return cleaned;
        }

        protected string RemoveImageVTags(string content)
        {
            var pattern = @"\s+v:\w+=""[^""]+""";

            return Regex.Replace(content, pattern, "");
        }
    }
}
