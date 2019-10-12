using System;
using Microsoft.AspNetCore.Mvc;
using PasteItCleaned.Cleaners;
using PasteItCleaned.Helpers;

namespace PasteItCleaned.Controllers
{
    [Route("api/v1/notify")]
    [ApiController]
    public class PasteNotifyController : ControllerBase
    {
        // POST api/v1/notify
        [HttpPost()]
        public ActionResult<string> Post([FromBody] NotifyObject obj)
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

                            AccountHelper.DecreaseBalance(objApiKey.ClientId, pasteType);
                            DbHelper.InsertHit(objApiKey.ClientId, pasteType);
                        }
                        else
                            return ErrorHelper.GetApiKeyDomainNotConfigured(apiKey, domain);
                    }
                    else
                        return ErrorHelper.GetApiKeyInvalid(apiKey);
                }

                return ErrorHelper.GetApiKeyAbsent();
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError(ex);
            }

            return "";
        }
    }

    public class NotifyObject
    {
        public string pasteType { get; set; }
    }
}
