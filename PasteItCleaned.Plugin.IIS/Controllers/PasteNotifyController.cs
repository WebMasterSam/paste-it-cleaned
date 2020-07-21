using System;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using PasteItCleaned.Core.Entities;
using PasteItCleaned.Core.Helpers;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using PasteItCleaned.Plugin.Controllers.Entities;
using PasteItCleaned.Plugin.Helpers;

namespace PasteItCleaned.Plugin.Controllers
{
    [ApiController]
    [Route("v1/notify")]
    [EnableCors("Default")]
    public class PasteNotifyController : BaseController
    {
        private readonly IHitService _hitService;
        private readonly IHitDailyService _hitDailyService;

        public PasteNotifyController(IApiKeyService apiKeyService, IClientService clientService, IConfigService configService, IDomainService domainService, IErrorService errorService, IHitService hitService, IHitDailyService hitDailyService, ILogger<PasteNotifyController> logger) : base(apiKeyService, clientService, configService, domainService, errorService, logger)
        {
            this._hitService = hitService;
            this._hitDailyService = hitDailyService;
        }

        // POST v1/notify
        [HttpPost()]
        public ActionResult Post([FromBody] NotifyObject obj)
        {
            var pasteType = "";

            try
            {
                var apiKey = this.GetApiKeyFromHeaders(this.HttpContext);

                pasteType = obj.pasteType;

                if (this.ApiKeyPresent(apiKey))
                {
                    var objApiKey = this.GetApiKeyFromDb(apiKey);
                    var domain = HostHelper.GetHostFromHeaders(this.HttpContext);

                    if (this.ApiKeyValid(objApiKey))
                    {
                        if (this.ApiKeyFitsWithDomain(objApiKey, domain))
                        {
                            var clientId = objApiKey != null ? objApiKey.ClientId : Guid.Empty;

                            if (clientId != Guid.Empty)
                            {
                                var pasteTypeObj = obj.pasteType.Trim().ToLower() == "image" ? SourceType.Image : SourceType.Text;
                                var ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                                var referer = Request.Headers["Referer"].ToString();
                                var userAgent = Request.Headers["User-Agent"].ToString();
                                var hitHash = _hitService.GetByHash(clientId, DateTime.UtcNow.Date, obj.hash);
                                var price = 0.0M;

                                if (hitHash == null)
                                {
                                    price = PricingHelper.GetHitPrice(clientId, pasteTypeObj);
                                    this.DecreaseBalance(clientId, price);
                                }

                                _hitService.Create(new Hit { ClientId = clientId, Date = DateTime.UtcNow, Hash = obj.hash, Ip = ip, Price = price, Referer = referer, UserAgent = userAgent, Type = pasteTypeObj.ToString() });
                                _hitDailyService.CreateOrIncrease(clientId, DateTime.UtcNow.Date, pasteTypeObj.ToString(), price);
                            }

                            return Ok(new PluginSuccess(""));
                        }
                        else
                            return Ok(new PluginError(ErrorHelper.GetApiKeyDomainNotConfigured(apiKey, domain)));
                    }
                    else
                        return Ok(new PluginError(ErrorHelper.GetApiKeyInvalid(apiKey)));
                }

                return Ok(new PluginError(ErrorHelper.GetApiKeyAbsent()));
            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }

            return Ok(new PluginSuccess(""));
        }
    }

    public class NotifyObject
    {
        public int hash { get; set; }
        public string pasteType { get; set; }
    }
}
