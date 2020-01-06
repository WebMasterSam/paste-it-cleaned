using PasteItCleaned.Backend.Core.Models;
using System;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface IHitDailyService
    {
        Task<HitDaily> Get(Guid clientId, DateTime date);
        Task<HitDaily> CreateHitDaily(HitDaily hitDaily);
        Task IncreaseHitDaily(HitDaily hitDaily, string type, decimal price);
    }
}
