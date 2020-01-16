using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Core.Services
{
    public interface IHitDailyService
    {
        List<HitDaily> List(Guid clientId, DateTime startDate, DateTime endDate);

        //HitDaily GetByDate(Guid clientId, DateTime date);

        HitDaily CreateOrIncrease(Guid clientId, DateTime date, string type, decimal price);

        void DeleteByDate(DateTime priorTo);
    }
}