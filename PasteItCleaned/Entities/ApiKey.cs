using System;
using System.Collections.Generic;

namespace PasteItCleaned.Entities
{
    public class ApiKey
    {
        public string Key { get; set; }
        public Guid ClientId { get; set; }
        public DateTime ExpiresOn { get; set; }
        public List<string> Domains { get; set; }
    }
}
