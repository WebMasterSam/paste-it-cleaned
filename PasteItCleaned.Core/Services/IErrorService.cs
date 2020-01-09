using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Core.Services
{
    public interface IErrorService
    {
        Task<IEnumerable<Error>> GetAllByClientId(Guid clientId);
        Task<Error> CreateError(Error error);
        Task DeleteErrors(DateTime olderThan);
    }
}
