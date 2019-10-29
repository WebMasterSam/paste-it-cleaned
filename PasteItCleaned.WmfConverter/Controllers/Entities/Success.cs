namespace PasteItCleaned.WmfConverter.Controllers.Entities
{
    public class Success
    {
        public Success(string base64)
        {
            this.Base64 = base64;
        }

        public string Base64 { get; set; }
    }
}
