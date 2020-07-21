namespace PasteItCleaned.Plugin.Controllers.Entities
{
    public class PluginError
    {
        public PluginError(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }
        public string Exception { get { return "editor.alert.failure"; } }
    }
}
