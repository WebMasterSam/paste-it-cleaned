using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using PasteItCleaned.Core.Helpers;
using System;

namespace PasteItCleaned.Backend.Common.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;
        private readonly IClientService _clientService;
        private readonly IUserService _userService;

        public BaseController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, ILogger logger)
        {
            this._apiKeyService = apiKeyService;
            this._clientService = clientService;
            this._userService = userService;
            this.Log = logger;
        }

        protected Client GetOrCreateClient(string authToken)
        {
            Log.LogDebug("BaseController::GetOrCreateClient");

            if (!string.IsNullOrWhiteSpace(authToken) && authToken.StartsWith("Bearer") && authToken.Contains("."))
            {
                var accessToken = Base64Helper.GetString(authToken.Replace("Bearer ", "").Split('.')[1]);
                var token = JObject.Parse(accessToken);
                var cognitoId = token.SelectToken("sub").Value<string>();
                var cognitoUsername = token.SelectToken("username").Value<string>();

                var user = _userService.GetByCognitoId(cognitoId);

                if (user == null)
                {
                    user = _userService.GetByCognitoUsername(cognitoUsername);
                }

                if (user == null)
                {
                    var client = _clientService.Create(new Client { CreatedOn = DateTime.Now });

                    _apiKeyService.Create(new ApiKey { ClientId = client.ClientId, CreatedOn = DateTime.Now, Key = ApiKeyHelper.GenerateApiKey(), ExpiresOn = DateTime.Now.AddYears(10) });
                    _userService.Create(new User { ClientId = client.ClientId, CreatedOn = DateTime.Now, CognitoId = cognitoId, CognitoUsername = cognitoUsername });
                }

                return _clientService.Get(user.ClientId);
            }

            return null;
        }

        protected ILogger Log { get; }
    }
}
