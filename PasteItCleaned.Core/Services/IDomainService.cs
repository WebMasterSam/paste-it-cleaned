using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Core.Services
{
    public interface IDomainService
    {
        Task<IEnumerable<Domain>> GetAllByApiKeyId(Guid apiKeyId);
        Task<Domain> GetById(Guid domainId);
        Task<Domain> GetByName(string name);
        Task<Domain> CreateDomain(Domain domain);
        Task UpdateDomain(Domain domainToBeUpdated, Domain domain);
        Task DeleteDomain(Domain domain);
    }
}
