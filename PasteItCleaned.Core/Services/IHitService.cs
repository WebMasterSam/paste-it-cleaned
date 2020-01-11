using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Core.Services
{
    public interface IHitService
    {
        //int Count(Guid clientId, string type, DateTime startDate, DateTime endDate);

        PagedList<Hit> List(Guid clientId, string type, DateTime startDate, DateTime endDate, int page, int pageSize);

        //Hit Get(Guid hitId);
        Hit GetByHash(Guid clientId, DateTime date, int hash);

        Hit Create(Hit hit);

        void DeleteByDate(DateTime priorTo);
    }
}
