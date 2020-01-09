using System;

namespace PasteItCleaned.Core.Models
{
    public class TimeZone
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public TimeSpan Offset { get; set; }
    }
}
