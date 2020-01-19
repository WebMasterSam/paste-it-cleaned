using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Core.Services;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("dashboard")]
    public class DashboardController : BaseController
    {
        private readonly IHitService _hitService;
        private readonly IHitDailyService _hitDailyService;
        private readonly IInvoiceService _invoiceService;

        public DashboardController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IHitService hitService, IHitDailyService hitDailyService, IInvoiceService invoiceService, IConfigService configService, ILogger<DashboardController> logger) : base(apiKeyService, clientService, userService, configService, logger)
        {
            this._hitService = hitService;
            this._hitDailyService = hitDailyService;
            this._invoiceService = invoiceService;
        }

        // GET dashboard/hits/
        [HttpGet("hits")]
        [ProducesResponseType(typeof(PagedListHitEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<PagedListHitEntity> GetDashboardHits([FromHeader]string authorization)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var hits = _hitService.List(client.ClientId, "", DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow, 1, 20);

                return Ok(hits);
            }
            catch (Exception ex)
            {
                return this.LogAndReturn500(ex);
            }
        }

        // GET dashboard/hits/
        [HttpGet("hits/daily")]
        [ProducesResponseType(typeof(ListHitDailyEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<ListHitDailyEntity> GetDashboardHitsDaily([FromHeader]string authorization, [FromQuery]DateTime startDate, [FromQuery]DateTime endDate)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var hits = _hitDailyService.List(client.ClientId, startDate, endDate);

                return Ok(hits);
            }
            catch (Exception ex)
            {
                return this.LogAndReturn500(ex);
            }
        }

        // GET dashboard/invoices/
        [HttpGet("invoices")]
        [ProducesResponseType(typeof(ListInvoiceEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<ListInvoiceEntity> GetDashboardInvoices([FromHeader]string authorization)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var invoices = _invoiceService.List(client.ClientId, 1, 20);

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return this.LogAndReturn500(ex);
            }
        }
    }
}
