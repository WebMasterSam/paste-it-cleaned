using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApiKeyService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public int Count(Guid clientId)
        {
            return _unitOfWork.ApiKeys.Count(clientId);
        }

        public ApiKey Create(ApiKey apiKey)
        {
            _unitOfWork.ApiKeys.Add(apiKey);
            _unitOfWork.Commit();

            return apiKey;
        }

        public void Delete(Guid apiKeyId)
        {
            _unitOfWork.ApiKeys.LogicalDelete(apiKeyId);

            _unitOfWork.Commit();
        }

        public ApiKey Get(Guid apiKeyId)
        {
            return _unitOfWork.ApiKeys.Get(apiKeyId);
        }

        public ApiKey GetByKey(string key)
        {
            return _unitOfWork.ApiKeys.GetByKey(key);
        }

        public List<ApiKey> List(Guid clientId)
        {
            return _unitOfWork.ApiKeys.List(clientId);
        }

        public ApiKey Update(ApiKey apiKey)
        {
            apiKey.UpdatedOn = DateTime.UtcNow;

            _unitOfWork.Commit();

            return apiKey;
        }
    }
}
