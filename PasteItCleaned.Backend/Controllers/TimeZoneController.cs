using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Core.Services;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("timezones")]
    public class TimeZoneController : BaseController
    {
        private ITimeZoneService _timeZoneService = null;

        public TimeZoneController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, ITimeZoneService timeZoneService, IConfigService configService, ILogger<DashboardController> logger) : base(apiKeyService, clientService, userService, configService, logger)
        {
            this._timeZoneService = timeZoneService;
        }

        // GET timezones
        [HttpGet()]
        [ProducesResponseType(typeof(ListTimeZoneEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<ListTimeZoneEntity> GetTimeZones()
        {
            try
            {
                var timeZones = _timeZoneService.GetAll();

                return Ok(timeZones);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }

        // GET timezones/{country}/
        [HttpGet("{country}")]
        [ProducesResponseType(typeof(ListTimeZoneEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<ListTimeZoneEntity> GetTimeZonesByCountry(string country)
        {
            try
            {
                var timeZones = _timeZoneService.GetAll(country);

                return Ok(timeZones);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }
    }
}
