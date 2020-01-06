using PasteItCleaned.Backend.Core;
using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApiKeyService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<ApiKey> CreateApiKey(ApiKey apiKey)
        {
            await _unitOfWork.ApiKeys.AddAsync(apiKey);
            await _unitOfWork.CommitAsync();

            return apiKey;
        }

        public async Task DeleteApiKey(ApiKey apiKey)
        {
            _unitOfWork.ApiKeys.LogicalDelete(apiKey);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ApiKey>> GetAllByClientId(Guid clientId)
        {
            return await _unitOfWork.ApiKeys.GetAllByParentIdAsync(clientId);
        }

        public async Task<ApiKey> GetById(Guid apiKeyId)
        {
            return await _unitOfWork.ApiKeys.GetByIdAsync(apiKeyId);
        }

        public async Task<ApiKey> GetByKey(string key)
        {
            return await _unitOfWork.ApiKeys.GetByKeyAsync(key);
        }

        public async Task UpdateApiKey(ApiKey apiKeyToBeUpdated, ApiKey apiKey)
        {
            apiKeyToBeUpdated.Domains = apiKey.Domains;
            apiKeyToBeUpdated.ExpiresOn = apiKey.ExpiresOn;

            await _unitOfWork.CommitAsync();
        }
    }
}
