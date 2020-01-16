using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System;
using System.Linq;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class ErrorRepository : Repository<Error>, IErrorRepository
    {
        public ErrorRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public int Count(Guid clientId)
        {
            return Context.Errors
                .Where(m => m.ClientId == clientId)
                .Count();
        }

        public void Delete(Guid errorId)
        {
            var entities = Context.Errors
                .Where(m => m.ErrorId == errorId);

            Context.Errors.RemoveRange(entities);
        }

        public void DeleteByDate(DateTime priorTo)
        {
            var entities = Context.Errors
                .Where(m => m.CreatedOn < priorTo);

            Context.Errors.RemoveRange(entities);
        }

        public PagedList<Error> List(Guid clientId, int page, int pageSize)
        {
            var query = Context.Errors
                .Where(m => m.ClientId == clientId)
                .OrderByDescending(m => m.CreatedOn);

            return this.PagedList(query, page, pageSize);
        }
    }
}
