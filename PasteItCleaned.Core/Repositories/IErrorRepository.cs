using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IErrorRepository : IRepository<Error>
    {
        int Count(Guid clientId);
        List<Error> List(Guid clientId);

        void Delete(Guid errorId);
        void DeleteByDate(DateTime priorTo);
    }
}
