using System;

namespace PasteItCleaned.Backend.Core.Models
{
    public class TimeZone
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public TimeSpan Offset { get; set; }
    }
}
