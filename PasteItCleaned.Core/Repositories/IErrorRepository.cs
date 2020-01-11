using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IErrorRepository : IRepository<Error>
    {
        int Count(Guid clientId);
        PagedList<Error> List(Guid clientId, int page, int pageSize);

        void Delete(Guid errorId);
        void DeleteByDate(DateTime priorTo);
    }
}
