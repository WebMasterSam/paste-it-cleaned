using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using PasteItCleaned.Plugin.Cleaners;
using PasteItCleaned.Plugin.Cleaners.Office.Excel;
using PasteItCleaned.Plugin.Cleaners.Office.PowerPoint;
using PasteItCleaned.Plugin.Cleaners.Office.Word;
using PasteItCleaned.Plugin.Cleaners.Web;
using PasteItCleaned.Plugin.Controllers.Entities;
using PasteItCleaned.Plugin.Helpers;
using PasteItCleaned.Plugin.Localization;
using PasteItCleaned.Core.Services;
using PasteItCleaned.Core.Helpers;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Plugin.Cleaners.OpenOffice.All;

namespace PasteItCleaned.Plugin.Controllers
{
    [ApiController]
    [Route("v1/clean")]
    [EnableCors("Default")]
    public class PasteCleanController : BaseController
    {
        private static List<BaseCleaner> Cleaners = new List<BaseCleaner>();

        private readonly IHitService _hitService;
        private readonly IHitDailyService _hitDailyService;

        public PasteCleanController(IApiKeyService apiKeyService, IClientService clientService, IConfigService configService, IDomainService domainService, IErrorService errorService, IHitService hitService, IHitDailyService hitDailyService, ILogger<PasteCleanController> logger) : base(apiKeyService, clientService, configService, domainService, errorService, logger)
        {
            this._hitService = hitService;
            this._hitDailyService = hitDailyService;
        }

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
            Log.LogDebug("PasteCleanController::Post");

            var html = "";

            try
            {
                EnsureCleaners();

                var apiKey = this.GetApiKeyFromHeaders(this.HttpContext);

                html = obj.html;

                if (this.ApiKeyPresent(apiKey))
                {
                    var objApiKey = this.GetApiKeyFromDb(apiKey);
                    var domain = HostHelper.GetHostFromHeaders(this.HttpContext);

                    if (this.ApiKeyValid(objApiKey))
                    {
                        if (this.ApiKeyFitsWithDomain(objApiKey, domain))
                        {
                            var clientId = objApiKey != null ? objApiKey.ClientId : Guid.Empty;

                            if (this.BalanceIsSufficient(clientId))
                            {
                                var config = CleanerConfigHelper.GetConfigFromHeaders(this.HttpContext);
                                var configObj = this.GetConfigFromDb(clientId, config);
                                var ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                                var referer = Request.Headers["Referer"].ToString();
                                var userAgent = Request.Headers["User-Agent"].ToString();

                                return Ok(new PluginSuccess(Clean(html, obj.rtf, clientId, configObj, ip, referer, userAgent, obj.hash, obj.keepStyles)));
                            }
                            else
                                return Ok(new PluginError(ErrorHelper.GetAccountIsUnpaid()));
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
                
                if (ConfigHelper.GetAppSetting<bool>("Logging.Debug"))
                    return Ok(new PluginSuccess(html, ex.Message + ex.StackTrace));
                else
                    return Ok(new PluginSuccess(html));
            }
        }

        private string Clean(string html, string rtf, Guid clientId, Config config, string ip, string referer, string userAgent, int hash, bool keepStyles)
        {
            foreach (BaseCleaner cleaner in Cleaners)
            {
                if (cleaner.CanClean(html, rtf))
                {
                    if (clientId != Guid.Empty)
                    {
                        var hitHash = _hitService.GetByHash(clientId, DateTime.UtcNow.Date, hash);
                        var price = 0.0M;

                        try
                        {
                            if (hitHash == null)
                            {
                                price = PricingHelper.GetHitPrice(clientId, cleaner.GetSourceType());
                                this.DecreaseBalance(clientId, price);
                            }

                            _hitService.Create(new Hit { ClientId = clientId, Date = DateTime.UtcNow, Hash = hash, Ip = ip, Price = price, Referer = referer, UserAgent = userAgent, Type = cleaner.GetSourceType().ToString() });
                            _hitDailyService.CreateOrIncrease(clientId, DateTime.UtcNow.Date, cleaner.GetSourceType().ToString(), price);
                        }
                        catch (Exception ex)
                        {
                            this.LogError(ex);
                        }
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
                Cleaners.Add(new OpenOfficeAllCleaner());
                Cleaners.Add(new WebCleaner());
                Cleaners.Add(new RtfCleaner());
            }
        }
    }

    public class CleanObject
    {
        public int hash { get; set; }
        public string html { get; set; }
        public string rtf { get; set; }
        public bool keepStyles { get; set; }
    }
}
