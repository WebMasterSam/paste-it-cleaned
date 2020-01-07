using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Common.Cleaners;
using PasteItCleaned.Common.Cleaners.Office.Excel;
using PasteItCleaned.Common.Cleaners.Office.PowerPoint;
using PasteItCleaned.Common.Cleaners.Office.Word;
using PasteItCleaned.Common.Cleaners.Web;
using PasteItCleaned.Common.Controllers.Entities;
using PasteItCleaned.Common.Entities;
using PasteItCleaned.Common.Helpers;
using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Common.Controllers
{
    [ApiController]
    [Route("v1/clean")]
    [EnableCors("Default")]
    public class PasteCleanController : ControllerBase
    {
        private static List<BaseCleaner> Cleaners = new List<BaseCleaner>();

        // GET v1/clean
        [HttpGet()]
        public ActionResult Get()
        {
            return Ok(T.Get("App.Up"));
        }

        // POST v1/clean
        [HttpPost()]
        public ActionResult Post([FromBody] CleanObject obj)
        {
            var hash = "";
            var html = "";
            var rtf = "";
            var keepStyles = false;

            Console.WriteLine("PasteCleanController::Post");

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
                                var ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                                var referer = Request.Headers["Referer"].ToString();

                                return Ok(new Success(Clean(html, rtf, objApiKey.ClientId, configObj, ip, referer, hash, keepStyles)));
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

                return Ok(new Success(html));
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
