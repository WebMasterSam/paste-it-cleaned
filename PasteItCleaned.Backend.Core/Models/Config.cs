using System;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Config
    {
        public Guid ConfigId { get; set; }
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public bool EmbedExternalImages { get; set; }
        public bool RemoveEmptyTags { get; set; }
        public bool RemoveSpanTags { get; set; }
        public bool RemoveClassNames { get; set; }
        public bool RemoveIframes { get; set; }
        public bool RemoveTagAttributes { get; set; }
    }
}
