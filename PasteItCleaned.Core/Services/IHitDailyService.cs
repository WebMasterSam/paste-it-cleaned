using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Core.Services
{
    public interface IHitDailyService
    {
        Task<IEnumerable<HitDaily>> GetByDatesAsync(Guid clientId, DateTime startDate, DateTime endDate);
        Task<HitDaily> GetByDateAsync(Guid clientId, DateTime date);
        Task<HitDaily> CreateHitDailyAsync(HitDaily hitDaily);
        Task IncreaseHitDailyAsync(HitDaily hitDaily, string type, decimal price);
    }
}
