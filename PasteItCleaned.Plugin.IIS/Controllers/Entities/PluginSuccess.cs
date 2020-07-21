namespace PasteItCleaned.Plugin.Controllers.Entities
{
    public class PluginSuccess
    {
        public PluginSuccess(string content)
        {
            this.Content = content;
        }

        public PluginSuccess(string content, string exception)
        {
            this.Content = content;
            this.Exception = exception;
        }

        public string Content { get; set; }
        public string Exception { get; set; }
    }
}
