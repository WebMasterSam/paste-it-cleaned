namespace PasteItCleaned.Common.Controllers.Entities
{
    public class Success
    {
        public Success(string content)
        {
            this.Content = content;
        }

        public Success(string content, string exception)
        {
            this.Content = content;
            this.Exception = exception;
        }

        public string Content { get; set; }
        public string Exception { get; set; }
    }
}
