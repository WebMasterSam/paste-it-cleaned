using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System.Collections.Generic;
using System.Threading;
using TimeZoneConverter;
using TimeZoneNames;

namespace PasteItCleaned.Backend.Services
{
    public class TimeZoneService : ITimeZoneService
    {
        public TimeZoneService()
        {

        }

        public IEnumerable<TimeZone> GetAll()
        {
            var allZones = TZNames.GetDisplayNames("en-US");
            var zones = TZNames.GetDisplayNames(Thread.CurrentThread.CurrentCulture.ToString());
            var timeZones = new List<TimeZone>();

            foreach (var zone in allZones)
                foreach (var aZone in zones)
                    if (zone.Key == aZone.Key)
                        timeZones.Add(new TimeZone { Name = aZone.Key, DisplayName = aZone.Value, Offset = GetOffset(zone.Key) });

            return timeZones;
        }

        public IEnumerable<TimeZone> GetAll(string countryCode)
        {
            var allZones = TZNames.GetDisplayNames("en-US");
            var countryZones = TZNames.GetTimeZonesForCountry(countryCode, Thread.CurrentThread.CurrentCulture.ToString(), System.DateTimeOffset.UtcNow);
            var timeZones = new List<TimeZone>();

            foreach (var zone in allZones)
                foreach (var countryZone in countryZones)
                    if (zone.Key == countryZone.Key)
                        timeZones.Add(new TimeZone { Name = zone.Key, DisplayName = zone.Value, Offset = GetOffset(zone.Key) });
                
            return timeZones;
        }

        private System.TimeSpan GetOffset(string name)
        {
            System.TimeZoneInfo tz = TZConvert.GetTimeZoneInfo(name);
            
            return tz.GetUtcOffset(System.DateTime.UtcNow);
        }
    }
}
