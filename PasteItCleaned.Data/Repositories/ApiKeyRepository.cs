using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System.Linq;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class ApiKeyRepository : Repository<ApiKey>, IApiKeyRepository
    {
        public ApiKeyRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public int Count(Guid clientId)
        {
            return Context.ApiKeys
                .Where(m => m.ClientId == clientId)
                .Where(m => !m.Deleted)
                .Count();
        }

        public ApiKey Get(Guid apiKeyId)
        {
            return Context.ApiKeys
                .Where(m => m.ApiKeyId == apiKeyId)
                .FirstOrDefault();
        }

        public ApiKey GetByKey(string key)
        {
            return Context.ApiKeys
                .Where(m => m.Key == key)
                .FirstOrDefault();
        }

        public List<ApiKey> List(Guid clientId)
        {
            return Context.ApiKeys
                .Where(m => m.ClientId == clientId)
                .Where(m => !m.Deleted)
                .ToList();
        }

        public void LogicalDelete(Guid apiKeyId)
        {
            var entity = Context.ApiKeys
                .Where(m => m.ApiKeyId == apiKeyId)
                .FirstOrDefault();

            entity.Deleted = true;
            entity.UpdatedOn = DateTime.UtcNow;
        }
    }
}
