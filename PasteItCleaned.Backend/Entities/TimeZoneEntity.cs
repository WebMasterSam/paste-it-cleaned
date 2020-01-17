using System;

namespace PasteItCleaned.Backend.Entities
{
    public class TimeZoneEntity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public TimeSpan Offset { get; set; }
    }
}
