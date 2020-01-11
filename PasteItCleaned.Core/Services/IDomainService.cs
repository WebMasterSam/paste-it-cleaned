using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Core.Services
{
    public interface IDomainService
    {
        List<Domain> List(Guid apiKeyId);

        Domain Get(Guid domainId);
        Domain GetByName(string name);

        Domain Create(Domain domain);

        Domain Update(Domain domainToUpdate, Domain domain);

        void Delete(Domain domain);
    }
}
