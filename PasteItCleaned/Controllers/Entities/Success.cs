namespace PasteItCleaned.Controllers.Entities
{
    public class Success
    {
        public Success(string content, bool embedImages)
        {
            this.Content = content;
            this.EmbedExternalImages = embedImages;
        }

        public string Content { get; set; }
        public bool Exception { get { return false; } }
        public bool EmbedExternalImages { get; set; }
    }
}
