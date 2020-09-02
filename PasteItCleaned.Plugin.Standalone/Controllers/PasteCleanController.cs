using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Plugin.Cleaners;
using PasteItCleaned.Plugin.Cleaners.Office.Excel;
using PasteItCleaned.Plugin.Cleaners.Office.PowerPoint;
using PasteItCleaned.Plugin.Cleaners.Office.Word;
using PasteItCleaned.Plugin.Cleaners.Web;
using PasteItCleaned.Plugin.Controllers.Entities;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Helpers;
using PasteItCleaned.Plugin.Cleaners.OpenOffice.All;

namespace PasteItCleaned.Plugin.Controllers
{
    [ApiController]
    [Route("v1/clean")]
    [EnableCors("Default")]
    public class PasteCleanController : BaseController
    {
        private static List<BaseCleaner> Cleaners = new List<BaseCleaner>();

        public PasteCleanController() : base()
        {

        }

        // GET v1/clean
        [HttpGet()]
        public ActionResult Get()
        {
            return Ok("Plugin is up and ready to accept clean requests.");
        }

        // POST v1/clean
        [HttpPost()]
        public ActionResult Post([FromBody] CleanObject obj)
        {
            var html = "";

            try
            {
                var config = new Config();

                config.EmbedExternalImages = ConfigHelper.GetAppSetting("Features.EmbedExternalImages") == "true";
                config.RemoveClassNames = ConfigHelper.GetAppSetting("Features.RemoveClassNames") == "true";
                config.RemoveIframes = ConfigHelper.GetAppSetting("Features.RemoveIframes") == "true";
                config.RemoveSpanTags = ConfigHelper.GetAppSetting("Features.RemoveSpanTags") == "true";
                config.RemoveEmptyTags = ConfigHelper.GetAppSetting("Features.RemoveEmptyTags") == "true";
                config.RemoveTagAttributes = ConfigHelper.GetAppSetting("Features.RemoveTagAttributes") == "true";

                EnsureCleaners();

                html = obj.html;

                var ip = HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
                var referer = Request.Headers["Referer"].ToString();
                var userAgent = Request.Headers["User-Agent"].ToString();

                return Ok(new PluginSuccess(Clean(html, obj.rtf, config, ip, referer, userAgent, obj.hash, obj.keepStyles)));
            }
            catch (Exception ex)
            {
                this.LogError(ex);
                
                return Ok(new PluginSuccess(html));
            }
        }

        private string Clean(string html, string rtf, Config config, string ip, string referer, string userAgent, int hash, bool keepStyles)
        {
            foreach (BaseCleaner cleaner in Cleaners)
                if (cleaner.CanClean(html, rtf))
                    return cleaner.Clean(html, rtf, config, keepStyles);

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
