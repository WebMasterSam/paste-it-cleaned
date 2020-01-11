using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Core.Services
{
    public interface IHitDailyService
    {
        //int Count(Guid clientId, string type, DateTime startDate, DateTime endDate);

        PagedList<HitDaily> List(Guid clientId, DateTime startDate, DateTime endDate, int page, int pageSize);

        //HitDaily GetByDate(Guid clientId, DateTime date);

        HitDaily CreateOrIncrease(Guid clientId, DateTime date, string type, decimal price);

        void DeleteByDate(DateTime priorTo);
    }
}