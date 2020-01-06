using System;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Error
    {
        public Guid ErrorId { get; set; }
        public Guid ClientId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
