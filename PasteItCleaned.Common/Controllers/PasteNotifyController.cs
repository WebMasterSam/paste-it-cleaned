using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using PasteItCleaned.Backend.Core.Services;
using PasteItCleaned.Common.Cleaners;
using PasteItCleaned.Common.Controllers.Entities;
using PasteItCleaned.Common.Helpers;

namespace PasteItCleaned.Common.Controllers
{
    [ApiController]
    [Route("v1/notify")]
    [EnableCors("Default")]
    public class PasteNotifyController : ControllerBase
    {
        private readonly IHitService _hitService;
        private readonly IHitDailyService _hitDailyService;

        public PasteNotifyController(IHitService hitService, IHitDailyService hitDailyService)
        {
            this._hitService = hitService;
            this._hitDailyService = hitDailyService;
        }

        // POST v1/notify
        [HttpPost()]
        public ActionResult Post([FromBody] NotifyObject obj)
        {
            var hash = "";
            var pasteType = "";

            try
            {
                var apiKey = ApiKeyHelper.GetApiKeyFromHeaders(this.HttpContext);

                hash = obj.hash;
                pasteType = obj.pasteType;

                if (ApiKeyHelper.ApiKeyPresent(apiKey))
                {
                    var objApiKey = ApiKeyHelper.GetApiKeyFromDb(apiKey);
                    var domain = HostHelper.GetHostFromHeaders(this.HttpContext);

                    if (ApiKeyHelper.ApiKeyValid(objApiKey))
                    {
                        if (ApiKeyHelper.ApiKeyFitsWithDomain(objApiKey, domain))
                        {
                            var pasteTypeObj = obj.pasteType.Trim().ToLower() == "image" ? SourceType.Image : SourceType.Text;
                            var ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                            var referer = Request.Headers["Referer"].ToString();
                            var hitHash = DbHelper.GetHitHash(objApiKey.ClientId, hash);
                            var price = 0.0M;

                            if (hitHash.Date < DateTime.UtcNow.Date)
                            {
                                price = AccountHelper.GetHitPrice(objApiKey.ClientId, pasteTypeObj);
                                AccountHelper.DecreaseBalance(objApiKey.ClientId, pasteTypeObj, price);
                                DbHelper.InsertHitHash(objApiKey.ClientId, hash);
                            }

                            DbHelper.InsertHit(objApiKey.ClientId, pasteTypeObj, ip, referer, price);

                            return Ok(new Success(""));
                        }
                        else
                            return Ok(new Error(ErrorHelper.GetApiKeyDomainNotConfigured(apiKey, domain)));
                    }
                    else
                        return Ok(new Error(ErrorHelper.GetApiKeyInvalid(apiKey)));
                }

                return Ok(new Error(ErrorHelper.GetApiKeyAbsent()));
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError(ex);
            }

            return Ok(new Success(""));
        }
    }

    public class NotifyObject
    {
        public string hash { get; set; }
        public string pasteType { get; set; }
    }
}
