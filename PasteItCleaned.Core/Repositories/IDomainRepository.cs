using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IDomainRepository : IRepository<Domain>
    {
        int Count(Guid apiKeyId);
        List<Domain> List(Guid apiKeyId);

        Domain Get(Guid domainId);
        Domain GetByName(Guid apiKeyId, string name);

        void LogicalDelete(Guid domainId);
    }
}
