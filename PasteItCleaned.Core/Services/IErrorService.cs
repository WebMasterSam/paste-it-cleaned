using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Core.Services
{
    public interface IErrorService
    {
        PagedList<Error> List(Guid clientId, int page, int pageSize);

        Error Create(Error error);

        void Delete(DateTime priorTo);
    }
}
