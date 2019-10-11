using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Entities
{
    public class ApiKey
    {
        public string Key { get; set; }
        public Guid ClientId { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
