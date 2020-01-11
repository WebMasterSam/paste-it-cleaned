using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IHitDailyRepository : IRepository<HitDaily>
    {
        int Count(Guid clientId, string type, DateTime startDate, DateTime endDate);

        PagedList<HitDaily> List(Guid clientId, DateTime startDate, DateTime endDate, int page, int pageSize);

        HitDaily AddOrUpdate(HitDaily hitDaily);

        void DeleteByDate(DateTime priorTo);
    }
}
