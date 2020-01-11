using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IHitRepository : IRepository<Hit>
    {
        int Count(Guid clientId, string type, DateTime startDate, DateTime endDate);

        PagedList<Hit> List(Guid clientId, string type, DateTime startDate, DateTime endDate, int page, int pageSize);

        Hit GetByHash(Guid clientId, DateTime date, int hash);

        void DeleteByDate(DateTime priorTo);
    }
}
