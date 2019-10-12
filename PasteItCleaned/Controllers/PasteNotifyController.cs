using System;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using PasteItCleaned.Cleaners;
using PasteItCleaned.Controllers.Entities;
using PasteItCleaned.Helpers;

namespace PasteItCleaned.Controllers
{
    [Route("api/v1/notify")]
    [ApiController]
    public class PasteNotifyController : ControllerBase
    {
        // POST api/v1/notify
        [HttpPost()]
        public ActionResult Post([FromBody] NotifyObject obj)
        {
            try
            {
                var apiKey = ApiKeyHelper.GetApiKeyFromHeaders(this.HttpContext);

                if (ApiKeyHelper.ApiKeyPresent(apiKey))
                {
                    var objApiKey = ApiKeyHelper.GetApiKeyFromDb(apiKey);
                    var domain = this.HttpContext.Request.Host.Host.ToLower().Trim();

                    if (ApiKeyHelper.ApiKeyValid(objApiKey))
                    {
                        if (ApiKeyHelper.ApiKeyFitsWithDomain(objApiKey, domain))
                        {
                            var pasteType = obj.pasteType.Trim().ToLower() == "image" ? SourceType.Image : SourceType.Text;
                            var ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                            var referer = Request.Headers["Referer"].ToString();

                            AccountHelper.DecreaseBalance(objApiKey.ClientId, pasteType);
                            DbHelper.InsertHit(objApiKey.ClientId, pasteType, ip, referer);

                            return Ok(new Success("", false));
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

            return Ok(new Success("", false));
        }
    }

    public class NotifyObject
    {
        public string pasteType { get; set; }
    }
}
