﻿using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System.Linq;
using System;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class HitDailyRepository : Repository<HitDaily>, IHitDailyRepository
    {
        public HitDailyRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public int Count(Guid clientId, string type, DateTime startDate, DateTime endDate)
        {
            var rows = Context.HitsDaily
                .Where(m => m.ClientId == clientId)
                .Where(m => m.Date >= startDate)
                .Where(m => m.Date <= endDate);

            switch (type.ToLower())
            {
                case "excel": return rows.Sum(m => m.CountExcel);
                case "image": return rows.Sum(m => m.CountImage);
                case "other": return rows.Sum(m => m.CountOther);
                case "powerpoint": return rows.Sum(m => m.CountPowerPoint);
                case "text": return rows.Sum(m => m.CountText);
                case "web": return rows.Sum(m => m.CountWeb);
                case "word": return rows.Sum(m => m.CountWord);
            }

            return rows.Sum(m => m.CountExcel + m.CountImage + m.CountOther + m.CountPowerPoint + m.CountText + m.CountWeb + m.CountWord);
        }

        public void DeleteByDate(DateTime priorTo)
        {
            var entities = Context.HitsDaily
                .Where(m => m.Date < priorTo);

            Context.HitsDaily.RemoveRange(entities);
        }

        public HitDaily Get(Guid clientId, DateTime date)
        {
            return Context.HitsDaily
                .Where(m => m.ClientId == clientId)
                .Where(m => m.Date == date.Date)
                .FirstOrDefault();
        }

        public PagedList<HitDaily> List(Guid clientId, DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            var query = Context.HitsDaily
                .Where(m => m.ClientId == clientId)
                .Where(m => m.Date >= startDate)
                .Where(m => m.Date <= endDate);

            return this.PagedList(query, page, pageSize);
        }
    }
}
