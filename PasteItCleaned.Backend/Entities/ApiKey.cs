using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Entities
{
    public class ApiKey
    {
        public Guid ApiKeyId { get; set; }
        public string Key { get; set; }
        public DateTime ExpiresOn { get; set; }
        public List<Domain> Domains { get; set; }
    }
}
