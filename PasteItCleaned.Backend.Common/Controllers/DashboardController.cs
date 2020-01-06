using System;

using Microsoft.AspNetCore.Mvc;
using PasteItCleaned.Backend.Common.Helpers;
using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("dashboard")]
    public class DashboardController : ControllerBase
    {
        // GET dashboard/hits/
        [HttpGet("hits")]
        public ActionResult GetHits([FromHeader]string authorization)
        {
            Console.WriteLine("DashboardController::GetHits");

            var client = SessionHelper.GetCurrentClient(authorization);

            // Use QS to filter by date, limit, sort, etc.
            return Ok("{ 'asdf' : 'ff' }");
        }

        // GET dashboard/invoices/
        [HttpGet("invoices")]
        public ActionResult GetInvoices([FromHeader]string authorization)
        {
            Console.WriteLine("DashboardController::GetInvoices");

            var client = SessionHelper.GetCurrentClient(authorization);

            // Use QS to filter by date, limit, sort, etc.
            return Ok(T.Get("App.Up"));
        }
    }
}
