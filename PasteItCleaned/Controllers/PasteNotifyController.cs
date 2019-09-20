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
                            DbHelper.SaveStat(SourceType.Text);
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
                // Error handling
            }

            return "";
        }
    }

    public class NotifyObject
    {
        public string value { get; set; }
    }
}
