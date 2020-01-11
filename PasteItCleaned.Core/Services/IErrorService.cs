using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Core.Services
{
    public interface IErrorService
    {
        PagedList<Error> List(Guid clientId);

        Error Create(Error error);

        void Delete(DateTime priorTo);
    }
}
