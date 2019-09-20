using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PasteItCleaned.Cleaners
{
    public class HtmlCleaner : BaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.Unknown;
        }

        public override string Clean(string content)
        {
            var cleaned = content;

            cleaned = this.RemoveComments(cleaned);
            cleaned = this.RemoveScriptTags(cleaned);

            cleaned = base.Clean(content);

            return cleaned;
        }

        private string RemoveComments(string content)
        {
            var Pattern = "<!--.*?-->";

            return Regex.Replace(content, Pattern, "");
        }

        private string RemoveScriptTags(string content)
        {
            var PAttern = "<script.*?>.*?</script>";

            return Regex.Replace(content, PAttern, "");
        }
    }
}
