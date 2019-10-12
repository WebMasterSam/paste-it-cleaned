using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Cleaners;
using PasteItCleaned.Cleaners.Office.Excel;
using PasteItCleaned.Cleaners.Office.PowerPoint;
using PasteItCleaned.Cleaners.Office.Word;
using PasteItCleaned.Cleaners.Web;
using PasteItCleaned.Helpers;

namespace PasteItCleaned.Controllers
{
    [Route("api/v1/clean")]
    [ApiController]
    public class PasteCleanController : ControllerBase
    {
        private static List<BaseCleaner> Cleaners = new List<BaseCleaner>();

        // POST api/v1/clean
        [HttpPost()]
        public ActionResult<string> Post([FromBody] CleanObject obj)
        {
            var content = "";

            try
            {
                EnsureCleaners();
                
                content = obj.value;

                var apiKey = ApiKeyHelper.GetApiKeyFromHeaders(this.HttpContext);

                if (ApiKeyHelper.ApiKeyPresent(apiKey))
                {
                    var objApiKey = ApiKeyHelper.GetApiKeyFromDb(apiKey);
                    var domain = this.HttpContext.Request.Host.Host.ToLower().Trim();

                    if (ApiKeyHelper.ApiKeyValid(objApiKey))
                    {
                        if (ApiKeyHelper.ApiKeyFitsWithDomain(objApiKey, domain))
                        {
                            if (AccountHelper.BalanceIsSufficient(objApiKey.ClientId))
                            {
                                return Clean(content, objApiKey.ClientId);
                            }
                            else
                                return ErrorHelper.GetAccountIsUnpaid();
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

                return content;
            }
        }

        private string Clean(string content, Guid clientId)
        {
            foreach (BaseCleaner cleaner in Cleaners)
            {
                if (cleaner.CanClean(content))
                {
                    var config = CleanerConfigHelper.GetConfigFromHeaders(this.HttpContext);
                    var configObj = CleanerConfigHelper.GetConfigFromDb(clientId, config);

                    try { DbHelper.InsertHit(clientId, cleaner.GetSourceType()); } catch (Exception ex) { ErrorHelper.LogError(ex); }
                    try { AccountHelper.DecreaseBalance(clientId, cleaner.GetSourceType()); } catch (Exception ex) { ErrorHelper.LogError(ex); }

                    return cleaner.Clean(content, configObj);
                }
            }

            return content;
        }

        private void EnsureCleaners()
        {
            if (Cleaners.Count == 0)
            {
                Cleaners.Add(new OfficeExcelCleaner());
                Cleaners.Add(new OfficeWordCleaner());
                Cleaners.Add(new OfficePowerPointCleaner());
                Cleaners.Add(new WebCleaner());
            }
        }
    }

    public class CleanObject
    {
        public string value { get; set; }
    }
}
