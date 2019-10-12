using PasteItCleaned.Entities;

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

        public override string Clean(string content, Config config)
        {
            var cleaned = content;

            if (config.GetConfigValue("removeIframes", true))
                cleaned = base.SafeExec(base.RemoveIframes, cleaned);

            if (config.GetConfigValue("removeTagAttributes", true))
                cleaned = base.SafeExec(this.RemoveUselessAttributes, cleaned);

            if (config.GetConfigValue("removeClassNames", true))
            {
                cleaned = base.SafeExec(base.AddInlineStyles, cleaned);
                cleaned = base.SafeExec(this.RemoveClassAttributes, cleaned);
            }

            cleaned = base.Clean(cleaned, config);

            return cleaned;
        }
    }
}
