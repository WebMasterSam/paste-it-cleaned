using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System.Linq;
using System;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class HitRepository : Repository<Hit>, IHitRepository
    {
        public HitRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public int Count(Guid clientId, string type, DateTime startDate, DateTime endDate)
        {
            return Context.Hits
                .Where(m => m.ClientId == clientId)
                .Where(m => m.Type == type || string.IsNullOrWhiteSpace(type))
                .Where(m => m.Date >= startDate)
                .Where(m => m.Date <= endDate)
                .Count();
        }

        public void DeleteByDate(DateTime priorTo)
        {
            var entities = Context.Hits
                .Where(m => m.Date < priorTo);

            Context.Hits.RemoveRange(entities);
        }

        public Hit GetByHash(Guid clientId, DateTime date, int hash)
        {
            return Context.Hits
                .Where(m => m.Hash == hash)
                .Where(m => m.Date >= date.Date)
                .Where(m => m.Date <= date.Date.AddDays(1).AddSeconds(-1))
                .FirstOrDefault();
        }

        public PagedList<Hit> List(Guid clientId, string type, DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            var query = Context.Hits
                .Where(m => m.ClientId == clientId)
                .Where(m => m.Type == type || string.IsNullOrWhiteSpace(type))
                .Where(m => m.Date >= startDate)
                .Where(m => m.Date <= endDate);

            return this.PagedList(query, page, pageSize);
        }
    }
}
