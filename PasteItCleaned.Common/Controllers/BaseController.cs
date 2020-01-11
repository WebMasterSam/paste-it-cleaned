using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using PasteItCleaned.Core.Helpers;

namespace PasteItCleaned.Plugin.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;
        private readonly IClientService _clientService;
        private readonly IConfigService _configService;
        private readonly IDomainService _domainService;
        private readonly IErrorService _errorService;

        public BaseController(IApiKeyService apiKeyService, IClientService clientService, IConfigService configService, IDomainService domainService, IErrorService errorService, ILogger logger)
        {
            this._apiKeyService = apiKeyService;
            this._clientService = clientService;
            this._configService = configService;
            this._domainService = domainService;
            this._errorService = errorService;
            this.Log = logger;
        }

        protected bool ApiKeyPresent(string apiKey)
        {
            if (!ConfigHelper.GetAppSetting<bool>("Features.ApiKeyValidation"))
                return true;

            return !string.IsNullOrWhiteSpace(apiKey);
        }

        protected bool ApiKeyValid(ApiKey apiKey)
        {
            if (!ConfigHelper.GetAppSetting<bool>("Features.ApiKeyValidation"))
                return true;

            if (apiKey != null)
                if (apiKey.ExpiresOn.Date > DateTime.UtcNow.Date)
                    return true;

            return false;
        }

        protected bool ApiKeyFitsWithDomain(ApiKey apiKey, string domain)
        {
            if (!ConfigHelper.GetAppSetting<bool>("Features.ApiKeyValidation"))
                return true;

            if (domain == "localhost" || domain == "127.0.0.1")
                return true;

            if (apiKey != null)
            {
                var oDomain = _domainService.GetByName(apiKey.ApiKeyId, domain.Trim().ToLower());

                if (oDomain != null)
                    return true;
            }

            return false;
        }

        protected string GetApiKeyFromHeaders(HttpContext context)
        {
            var apiKey = context.Request.Headers["ApiKey"];

            return apiKey.Count > 0 ? apiKey[0] : "";
        }

        protected ApiKey GetApiKeyFromDb(string apiKey)
        {
            return _apiKeyService.GetByKey(apiKey);
        }

        protected bool BalanceIsSufficient(Guid clientId)
        {
            if (!ConfigHelper.GetAppSetting<bool>("Features.AccountValidation"))
                return true;

            var client = _clientService.Get(clientId);

            return client.Balance > 0;
        }

        protected void DecreaseBalance(Guid clientId, decimal decreaseBy)
        {
            _clientService.DecreaseBalance(clientId, decreaseBy);
        }


        public Config GetConfigFromDb(Guid clientId, string config)
        {
            var configs = _configService.List(clientId);
            var configObj = configs.Find((c) => { return c.Name.ToLower().Trim() == config.ToLower().Trim(); });
            var configDefaultObj = configs.Find((c) => { return c.Name.ToLower().Trim() == "default"; });

            return configObj != null ? configObj : configDefaultObj;
        }


        protected void LogError(Exception ex)
        {
            LogError(Guid.Empty, ex);
        }

        protected void LogError(Guid clientId, Exception ex)
        {
            try
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                _errorService.Create(new Error { ClientId = clientId, CreatedOn = DateTime.UtcNow, Message = ex.Message, StackTrace = ex.StackTrace});
            }
            catch { }

            if (ex.InnerException != null)
                LogError(clientId, ex.InnerException);
        }


        protected ILogger Log { get; }
    }
}
