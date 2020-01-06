using System;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Hit
    {
        public Guid HitId { get; set; }
        public Guid ClientId { get; set; }
        public int Hash { get; set; }
        public string Ip { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public string Referer { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
