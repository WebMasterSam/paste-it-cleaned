using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Cleaners;
using PasteItCleaned.Cleaners.Office.Excel;
using PasteItCleaned.Cleaners.Office.PowerPoint;
using PasteItCleaned.Cleaners.Office.Word;
using PasteItCleaned.Cleaners.Web;
using PasteItCleaned.Controllers.Entities;
using PasteItCleaned.Entities;
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
        public ActionResult Post([FromBody] CleanObject obj)
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
                                var config = CleanerConfigHelper.GetConfigFromHeaders(this.HttpContext);
                                var configObj = CleanerConfigHelper.GetConfigFromDb(objApiKey.ClientId, config);
                                var embedImages = configObj != null ? configObj.GetConfigValue("embedExternalImages", false) : false;
                                var ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                                var referer = Request.Headers["Referer"].ToString();

                                return Ok(new Success(Clean(content, objApiKey.ClientId, configObj, ip, referer), embedImages));
                            }
                            else
                                return Ok(new Error(ErrorHelper.GetAccountIsUnpaid()));
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

                return Ok(new Success(content, false));
            }
        }

        private string Clean(string content, Guid clientId, Config config, string ip, string referer)
        {
            foreach (BaseCleaner cleaner in Cleaners)
            {
                if (cleaner.CanClean(content))
                {
                    try { DbHelper.InsertHit(clientId, cleaner.GetSourceType(), ip, referer); } catch (Exception ex) { ErrorHelper.LogError(ex); }
                    try { AccountHelper.DecreaseBalance(clientId, cleaner.GetSourceType()); } catch (Exception ex) { ErrorHelper.LogError(ex); }

                    return cleaner.Clean(content, config);
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
