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

        public override string Clean(string original, string content)
        {
            var cleaned = base.Clean(original, content);

            cleaned = this.RemoveComments(cleaned);
            cleaned = this.RemoveScriptTags(cleaned);

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
