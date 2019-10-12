using PasteItCleaned.Entities;
using System.Text.RegularExpressions;

namespace PasteItCleaned.Cleaners.Office
{
    public class OfficeBaseCleaner : HtmlCleaner
    {
        public override string Clean(string content, Config config)
        {
            var cleaned = content;

            cleaned = base.SafeExec(base.ParseWithHtmlAgilityPack, cleaned);

            cleaned = base.SafeExec(this.RemoveUselessStyles, cleaned);
            cleaned = base.SafeExec(base.AddInlineStyles, cleaned);
            cleaned = base.SafeExec(this.RemoveUselessAttributes, cleaned);
            cleaned = base.SafeExec(base.RemoveUselessTags, cleaned);

            //cleaned = base.SafeExec(this.RemoveEmptyParagraphs, cleaned);

            cleaned = base.Clean(cleaned, config);

            return cleaned;
        }

        // Gérer les versions plus/moins récentes de Office



        /* 
         Configs :
         remove classnames for web only
         remove span tags and leave text
         images : remove | convert to inline voir commentaire plus bas
         tags : remove empty
         remove whitespace tags
         remove iframes
         attribute tags : remove all | remove empty
         links : remove
         tables : remove | leave text only

         embed external images (file:/// are always embed, but http:// are not embeded by default)
         ramener la config via objet json dans le callback, et traiter les images coté client
        */
    }
}
