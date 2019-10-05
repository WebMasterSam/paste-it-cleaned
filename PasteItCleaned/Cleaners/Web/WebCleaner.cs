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

            cleaned = base.Clean(cleaned);

            return cleaned;
        }



        /* 
         Configs :
         remove classnames
         remove span tags and leave text
         images : remove | convert to inline
         tags : remove empty
         remove whitespace tags
         remove iframes
         attribute tags : remove all | remove empty
         links : remove
         tables : remove | leave text only
         */
    }
}
