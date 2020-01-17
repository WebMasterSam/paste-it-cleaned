using System;

namespace PasteItCleaned.Backend.Entities
{
    public class HitEntity
    {
        public Guid HitId { get; set; }
        public DateTime Date { get; set; }
        public string Ip { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public string Referer { get; set; }
    }
}
