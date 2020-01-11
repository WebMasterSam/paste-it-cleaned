using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IApiKeyRepository : IRepository<ApiKey>
    {
        int Count(Guid clientId);
        List<ApiKey> List(Guid clientId);

        ApiKey Get(Guid apiKeyId);
        ApiKey GetByKey(string key);

        void LogicalDelete(Guid apiKeyId);
    }
}
