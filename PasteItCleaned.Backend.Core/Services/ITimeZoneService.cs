using PasteItCleaned.Backend.Core.Models;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface ITimeZoneService
    {
        IEnumerable<TimeZone> GetAll();
        IEnumerable<TimeZone> GetAll(string countryCode);
    }
}
