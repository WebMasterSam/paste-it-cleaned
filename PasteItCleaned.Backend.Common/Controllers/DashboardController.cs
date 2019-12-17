using System;

using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    public class DashboardController : ControllerBase
    {
        // GET dashboard/hits/
        [HttpGet("hits")]
        public ActionResult GetHits()
        {
            // Use QS to filter by date, limit, sort, etc.
            return Ok(T.Get("App.Up"));
        }

        // GET dashboard/invoices/
        [HttpGet("invoices")]
        public ActionResult GetInvoices()
        {
            // Use QS to filter by date, limit, sort, etc.
            return Ok(T.Get("App.Up"));
        }
    }
}
