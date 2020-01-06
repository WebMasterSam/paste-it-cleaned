using PasteItCleaned.Backend.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface IApiKeyService
    {
        Task<IEnumerable<ApiKey>> GetAllByClientId(Guid clientId);
        Task<ApiKey> GetById(Guid apiKeyId);
        Task<ApiKey> GetByKey(string key);
        Task<ApiKey> CreateApiKey(ApiKey apiKey);
        Task UpdateApiKey(ApiKey apiKeyToBeUpdated, ApiKey apiKey);
        Task DeleteApiKey(ApiKey apiKey);
    }
}
