using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasteItCleaned.Backend.Core.Services;
using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("dashboard")]
    public class DashboardController : BaseController
    {
        private readonly IHitService _hitService;
        private readonly IInvoiceService _invoiceService;

        public DashboardController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IHitService hitService, IInvoiceService invoiceService) : base(apiKeyService, clientService, userService)
        {
            this._hitService = hitService;
            this._invoiceService = invoiceService;
        }

        // GET dashboard/hits/
        [HttpGet("hits")]
        public async Task<ActionResult> GetHits([FromHeader]string authorization)
        {
            Console.WriteLine("DashboardController::GetHits");

            var client = this.GetOrCreateClient(authorization);

            var hits = await _hitService.GetAllByClientIdAsync(Guid.Empty);

            return Ok(hits);
        }

        // GET dashboard/invoices/
        [HttpGet("invoices")]
        public ActionResult GetInvoices([FromHeader]string authorization)
        {
            Console.WriteLine("DashboardController::GetInvoices");

            var client = this.GetOrCreateClient(authorization);

            // Use QS to filter by date, limit, sort, etc.
            return Ok(T.Get("App.Up"));
        }
    }
}
