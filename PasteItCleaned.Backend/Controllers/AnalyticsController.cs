using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("analytics")]
    public class AnalyticsController : BaseController
    {
        private readonly IHitService _hitService;
        private readonly IHitDailyService _hitDailyService;

        public AnalyticsController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IHitService hitService, IHitDailyService hitDailyService, ILogger<AnalyticsController> logger) : base(apiKeyService, clientService, userService, logger)
        {
            this._hitService = hitService;
            this._hitDailyService = hitDailyService;
        }

        // GET analytics/hits/
        [HttpGet("hits")]
        [ProducesResponseType(typeof(PagedList<Hit>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<PagedList<Hit>> GetHits([FromHeader]string authorization, [FromQuery]int page, [FromQuery]int pageSize)
        {
            try
            {
                var client = this.GetOrCreateClient(authorization);
                var hits = _hitService.List(client.ClientId, "", DateTime.MinValue, DateTime.MaxValue, page, pageSize);

                return Ok(hits);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }

        // GET dashboard/hits/
        [HttpGet("hits/daily")]
        [ProducesResponseType(typeof(List<HitDaily>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<List<HitDaily>> GetDashboardHitsDaily([FromHeader]string authorization, [FromQuery]DateTime startDate, [FromQuery]DateTime endDate)
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
                Log.LogError("Exception", ex);
                return StatusCode(500);
            }
        }

    }

    public class AnalyticsRequest
    {
        public string any { get; set; }
    }
}
