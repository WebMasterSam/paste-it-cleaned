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
                if (ApiKeyHelper.ApiKeyPresent())
                {
                    if (ApiKeyHelper.ApiKeyValid())
                    {
                        if (ApiKeyHelper.ApiKeyFitsWithDomain())
                        {
                            var clientId = Guid.Empty;

                            DbHelper.InsertHit(clientId, obj.pasteType.Trim().ToLower() == "image" ? SourceType.Image : SourceType.Text);
                        }
                        else
                            return ErrorHelper.GetApiKeyDomainNotConfigured();
                    }
                    else
                        return ErrorHelper.GetApiKeyInvalid();
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
