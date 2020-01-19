using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using PasteItCleaned.Core.Helpers;
using System;
using System.Collections.Generic;
using PasteItCleaned.Backend.Entities;

namespace PasteItCleaned.Backend.Common.Controllers
{
    public class PagedListHitEntity : PagedList<HitEntity> { }
    public class ListHitDailyEntity : List<HitDailyEntity> { }
    public class ListInvoiceEntity : List<InvoiceEntity> { }
    public class ListApiKeyEntity : List<ApiKeyEntity> { }
    public class ListTimeZoneEntity : List<TimeZoneEntity> { }

    public class BaseController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;
        private readonly IClientService _clientService;
        private readonly IUserService _userService;
        private readonly IConfigService _configService;

        public BaseController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IConfigService configService, ILogger logger)
        {
            this._apiKeyService = apiKeyService;
            this._clientService = clientService;
            this._userService = userService;
            this._configService = configService;
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

                    _apiKeyService.Create(new ApiKey { ClientId = client.ClientId, CreatedOn = DateTime.UtcNow, Key = ApiKeyHelper.GenerateApiKey(), ExpiresOn = DateTime.Now.AddYears(10) });
                    _configService.Create(new Config { ClientId = client.ClientId, CreatedOn = DateTime.UtcNow, Name = "DEFAULT", EmbedExternalImages = false, RemoveClassNames = true, RemoveEmptyTags = true, RemoveIframes = true, RemoveSpanTags = true, RemoveTagAttributes = true });
                    _userService.Create(new User { ClientId = client.ClientId, CreatedOn = DateTime.UtcNow, CognitoId = cognitoId, CognitoUsername = cognitoUsername });
                }

                return _clientService.Get(user.ClientId);
            }

            return null;
        }

        protected StatusCodeResult LogAndReturn500(Exception ex)
        {
            Log.LogError("Exception", ex);

            return StatusCode(500);
        }

        protected ILogger Log { get; }
    }
}
