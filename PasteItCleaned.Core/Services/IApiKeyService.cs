using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Core.Services
{
    public interface IApiKeyService
    {
        int Count(Guid clientId);
        List<ApiKey> List(Guid clientId);

        ApiKey Get(Guid apiKeyId);
        ApiKey GetByKey(string key);

        ApiKey Create(ApiKey apiKey);

        ApiKey Update(ApiKey apiKey);

        void Delete(Guid apiKeyId);
    }
}
