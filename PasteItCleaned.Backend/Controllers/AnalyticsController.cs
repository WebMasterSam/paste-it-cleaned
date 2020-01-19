using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Core.Services;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("analytics")]
    public class AnalyticsController : BaseController
    {
        private readonly IHitService _hitService;
        private readonly IHitDailyService _hitDailyService;

        public AnalyticsController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IHitService hitService, IHitDailyService hitDailyService, IConfigService configService, ILogger<AnalyticsController> logger) : base(apiKeyService, clientService, userService, configService, logger)
        {
            this._hitService = hitService;
            this._hitDailyService = hitDailyService;
        }

        // GET analytics/hits/
        [HttpGet("hits")]
        [ProducesResponseType(typeof(PagedListHitEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<PagedListHitEntity> GetAnalyticsHits([FromHeader]string authorization, [FromQuery]int page, [FromQuery]int pageSize)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var hits = _hitService.List(client.ClientId, "", DateTime.MinValue, DateTime.MaxValue, page, pageSize);

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
        public ActionResult<ListHitDailyEntity> GetAnalyticsHitsDaily([FromHeader]string authorization, [FromQuery]DateTime startDate, [FromQuery]DateTime endDate)
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

    }

    public class AnalyticsRequest
    {
        public string any { get; set; }
    }
}
