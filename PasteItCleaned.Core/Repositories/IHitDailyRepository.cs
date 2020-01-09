using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IHitDailyRepository : IRepository<HitDaily>
    {
        Task<IEnumerable<HitDaily>> GetByDatesAsync(Guid clientId, DateTime startDate, DateTime endDate);
    }
}
