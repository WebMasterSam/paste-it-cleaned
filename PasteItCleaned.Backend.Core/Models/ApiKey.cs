using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Core.Models
{
    public class ApiKey
    {
        public Guid ApiKeyId { get; set; }
        public string Key { get; set; }
        public Guid ClientId { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
