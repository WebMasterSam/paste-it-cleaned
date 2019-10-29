using System;
using System.Collections.Generic;

namespace PasteItCleaned.Common.Entities
{
    public class Client
    {
        public Guid ClientId { get; set; }
        public List<string> ApiKeys { get; set; }
        public List<Config> Configs { get; set; }
        public Contact Contact { get; set; }
        public Business Business { get; set; }
        public Billing Billing { get; set; }
    }
}
