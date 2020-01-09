using System;
using System.Threading.Tasks;
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

        public AnalyticsController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IHitService hitService, IHitDailyService hitDailyService, ILogger<AnalyticsController> logger) : base(apiKeyService, clientService, userService, logger)
        {
            this._hitService = hitService;
            this._hitDailyService = hitDailyService;
        }

        // GET analytics/hits/
        [HttpGet("hits")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetHits([FromHeader]string authorization)
        {
            try
            {
                var client = await this.GetOrCreateClient(authorization);
                var hits = await _hitService.GetAllByClientIdAsync(client.ClientId);

                return Ok(hits);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }
    }

    public class AnalyticsRequest
    {
        public string any { get; set; }
    }
}
