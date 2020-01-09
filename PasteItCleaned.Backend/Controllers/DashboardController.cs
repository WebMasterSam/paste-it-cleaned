using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("dashboard")]
    public class DashboardController : BaseController
    {
        private readonly IHitService _hitService;
        private readonly IInvoiceService _invoiceService;

        public DashboardController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IHitService hitService, IInvoiceService invoiceService, ILogger<DashboardController> logger) : base(apiKeyService, clientService, userService, logger)
        {
            this._hitService = hitService;
            this._invoiceService = invoiceService;
        }

        // GET dashboard/hits/
        [HttpGet("hits")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetHits([FromHeader]string authorization)
        {
            var client = await this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var hits = await _hitService.GetAllByClientIdAsync(client.ClientId);

                return Ok(hits);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return StatusCode(500);
            }
        }

        // GET dashboard/invoices/
        [HttpGet("invoices")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetInvoices([FromHeader]string authorization)
        {
            var client = await this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var invoices = await _invoiceService.GetAllByClientIdAsync(client.ClientId);

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return StatusCode(500);
            }
        }
    }
}
