using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Cleaners.Office.Word
{
    public class OfficeWordCleaner : OfficeBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.Word;
        }

        public override bool CanClean(string content)
        {
            return content.ToLower().Contains("<meta name=Generator content=\"Microsoft Word".ToLower());
        }

        public override string Clean(string content)
        {
            var cleaned = content;

            // Gérer les versions plus récentes de word

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
