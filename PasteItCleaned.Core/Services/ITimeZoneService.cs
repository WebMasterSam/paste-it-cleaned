using PasteItCleaned.Core.Models;
using System.Collections.Generic;

namespace PasteItCleaned.Core.Services
{
    public interface ITimeZoneService
    {
        IEnumerable<TimeZone> GetAll();
        IEnumerable<TimeZone> GetAll(string countryCode);
    }
}
