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
    [Route("v1/clean")]
    [ApiController]
    public class PasteCleanController : ControllerBase
    {
        private static List<BaseCleaner> Cleaners = new List<BaseCleaner>();

        // POST api/v1/clean
        [HttpPost()]
        public ActionResult Post([FromBody] CleanObject obj)
        {
            var hash = "";
            var html = "";
            var rtf = "";
            var keepStyles = false;

            try
            {
                EnsureCleaners();

                hash = obj.hash;
                html = obj.html;
                rtf = obj.rtf;
                keepStyles = obj.keepStyles;

                var apiKey = ApiKeyHelper.GetApiKeyFromHeaders(this.HttpContext);
                
                if (ApiKeyHelper.ApiKeyPresent(apiKey))
                {
                    var objApiKey = ApiKeyHelper.GetApiKeyFromDb(apiKey);
                    var domain = HostHelper.GetHostFromHeaders(this.HttpContext);

                    if (ApiKeyHelper.ApiKeyValid(objApiKey))
                    {
                        if (ApiKeyHelper.ApiKeyFitsWithDomain(objApiKey, domain))
                        {
                            if (AccountHelper.BalanceIsSufficient(objApiKey.ClientId))
                            {
                                var config = CleanerConfigHelper.GetConfigFromHeaders(this.HttpContext);
                                var configObj = CleanerConfigHelper.GetConfigFromDb(objApiKey.ClientId, config);
                                var embedImages = configObj != null ? configObj.GetConfigValue("EmbedExternalImages", false) : false;
                                var ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                                var referer = Request.Headers["Referer"].ToString();

                                return Ok(new Success(Clean(html, rtf, objApiKey.ClientId, configObj, ip, referer, hash, keepStyles), embedImages));
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

                return Ok(new Success(html, false));
            }
        }

        private string Clean(string html, string rtf, Guid clientId, Config config, string ip, string referer, string hash, bool keepStyles)
        {
            foreach (BaseCleaner cleaner in Cleaners)
            {
                if (cleaner.CanClean(html))
                {
                    var hitHash = DbHelper.GetHitHash(clientId, hash);
                    var price = 0.0M;

                    try
                    { 
                        if (hitHash.Date < DateTime.UtcNow.Date)
                        {
                            price = AccountHelper.GetHitPrice(clientId, cleaner.GetSourceType());
                            AccountHelper.DecreaseBalance(clientId, cleaner.GetSourceType(), price);
                            DbHelper.InsertHitHash(clientId, hash);
                        }

                        DbHelper.InsertHit(clientId, cleaner.GetSourceType(), ip, referer, price);
                    }
                    catch (Exception ex)
                    {
                        ErrorHelper.LogError(ex);
                    }

                    return cleaner.Clean(html, rtf, config, keepStyles);
                }
            }

            return html;
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
        public string hash { get; set; }
        public string html { get; set; }
        public string rtf { get; set; }
        public bool keepStyles { get; set; }
    }
}
