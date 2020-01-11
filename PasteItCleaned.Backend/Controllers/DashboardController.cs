using System;
using System.Collections.Generic;
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
        private readonly IHitDailyService _hitDailyService;
        private readonly IInvoiceService _invoiceService;

        public DashboardController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IHitService hitService, IHitDailyService hitDailyService, IInvoiceService invoiceService, ILogger<DashboardController> logger) : base(apiKeyService, clientService, userService, logger)
        {
            this._hitService = hitService;
            this._hitDailyService = hitDailyService;
            this._invoiceService = invoiceService;
        }

        // GET dashboard/hits/
        [HttpGet("hits")]
        [ProducesResponseType(typeof(List<Hit>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<List<Hit>> GetHits([FromHeader]string authorization)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var hits = _hitService.List(client.ClientId, "", DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow, 0, 20);

                return Ok(hits);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return StatusCode(500);
            }
        }

        // GET dashboard/hits/
        [HttpGet("hits/daily")]
        [ProducesResponseType(typeof(List<Hit>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<List<HitDaily>> GetHitsDaily([FromHeader]string authorization, [FromQuery]DateTime startDate, [FromQuery]DateTime endDate)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var hits = _hitDailyService.List(client.ClientId, startDate, endDate, 0, 20);

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
        [ProducesResponseType(typeof(List<Invoice>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<List<Invoice>> GetInvoices([FromHeader]string authorization)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var invoices = _invoiceService.List(client.ClientId, 0, 20);

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
