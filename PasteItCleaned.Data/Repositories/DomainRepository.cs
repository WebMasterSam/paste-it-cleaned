using System.Linq;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class DomainRepository : Repository<Domain>, IDomainRepository
    {
        public DomainRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public int Count(Guid apiKeyId)
        {
            return Context.Domains
                .Where(m => m.ApiKeyId == apiKeyId)
                .Where(m => !m.Deleted)
                .Count();
        }

        public Domain Get(Guid domainId)
        {
            return Context.Domains
                .Where(m => m.DomainId == domainId)
                .FirstOrDefault();
        }

        public Domain GetByName(string name)
        {
            return Context.Domains
                .Where(m => m.Name == name)
                .Where(m => !m.Deleted)
                .FirstOrDefault();
        }

        public List<Domain> List(Guid apiKeyId)
        {
            return Context.Domains
                .Where(m => m.ApiKeyId == apiKeyId)
                .Where(m => !m.Deleted)
                .ToList();
        }

        public void LogicalDelete(Guid domainId)
        {
            var entity = Context.Domains
                .Where(m => m.DomainId == domainId)
                .FirstOrDefault();

            entity.Deleted = true;
            entity.UpdatedOn = DateTime.UtcNow;
        }
    }
}
