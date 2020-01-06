using PasteItCleaned.Backend.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface IDomainService
    {
        Task<IEnumerable<Domain>> GetAllByApiKeyId(Guid clientId);
        Task<Domain> GetById(Guid domainId);
        Task<Domain> GetByName(string name);
        Task<Domain> CreateApiKey(Domain domain);
        Task UpdateDomain(Domain domainToBeUpdated, Domain domain);
        Task DeleteDomain(Domain domain);
    }
}
