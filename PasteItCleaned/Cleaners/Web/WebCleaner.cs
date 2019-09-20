using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Cleaners.Web
{
    public class WebCleaner : HtmlCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.Web;
        }

        public override bool CanClean(string content)
        {
            return true;
        }

        public override string Clean(string content)
        {
            var cleaned = content;

            cleaned = base.Clean(content);

            return cleaned;
        }
    }
}
