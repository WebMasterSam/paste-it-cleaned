using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IHitDailyRepository : IRepository<HitDaily>
    {
        int Count(Guid clientId, string type, DateTime startDate, DateTime endDate);

        List<HitDaily> List(Guid clientId, DateTime startDate, DateTime endDate);

        HitDaily Get(Guid clientId, DateTime date);

        void DeleteByDate(DateTime priorTo);
    }
}
