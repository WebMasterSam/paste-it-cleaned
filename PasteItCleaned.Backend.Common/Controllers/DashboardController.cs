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
            Console.WriteLine("DashboardController::GetHits");

            // Use QS to filter by date, limit, sort, etc.
            return Ok("{ 'asdf' : 'ff' }");
        }

        // GET dashboard/invoices/
        [HttpGet("invoices")]
        public ActionResult GetInvoices()
        {
            Console.WriteLine("DashboardController::GetInvoices");

            // Use QS to filter by date, limit, sort, etc.
            return Ok(T.Get("App.Up"));
        }
    }
}
