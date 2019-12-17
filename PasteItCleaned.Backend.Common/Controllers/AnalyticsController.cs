using System;

using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    public class AnalyticsController : ControllerBase
    {
        // GET analytics/hits/
        [HttpGet("hits")]
        public ActionResult Get()
        {
            // Use QS to filter by date, limit, sort, etc.
            return Ok(T.Get("App.Up"));
        }
    }

    public class AnalyticsRequest
    {
        public string any { get; set; }
    }
}
