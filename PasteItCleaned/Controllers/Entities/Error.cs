namespace PasteItCleaned.Controllers.Entities
{
    public class Error
    {
        public Error(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }
        public bool Exception { get { return true; } }
    }
}
