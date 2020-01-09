using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class HitDailyRepository : Repository<HitDaily>, IHitDailyRepository
    {
        public HitDailyRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public Task<IEnumerable<HitDaily>> GetByDatesAsync(Guid clientId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
