using PasteItCleaned.Core.Models;
using System.Collections.Generic;

namespace PasteItCleaned.Core.Services
{
    public interface ITimeZoneService
    {
        List<TimeZone> GetAll();
        List<TimeZone> GetAll(string countryCode);
    }
}
