using System;

namespace PasteItCleaned.Backend.Entities
{
    public class TimeZone
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public TimeSpan Offset { get; set; }
    }
}
