using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasteItCleaned.Backend.Common.Helpers;
using PasteItCleaned.Backend.Core.Services;
using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IHitService _hitService;

        public DashboardController(IHitService hitService)
        {
            this._hitService = hitService;
        }

        // GET dashboard/hits/
        [HttpGet("hits")]
        public async Task<ActionResult> GetHits([FromHeader]string authorization)
        {
            Console.WriteLine("DashboardController::GetHits");

            var client = SessionHelper.GetCurrentClient(authorization);

            var hits = await _hitService.GetAllByClientIdAsync(Guid.Empty);

            return Ok(hits);

            // Use QS to filter by date, limit, sort, etc.
            //return Ok("{ 'asdf' : 'ff' }");
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
