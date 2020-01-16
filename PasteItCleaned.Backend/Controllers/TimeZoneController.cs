﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Core.Services;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("timezones")]
    public class TimeZoneController : BaseController
    {
        private ITimeZoneService _timeZoneService = null;

        public TimeZoneController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, ITimeZoneService timeZoneService, ILogger<DashboardController> logger) : base(apiKeyService, clientService, userService, logger)
        {
            this._timeZoneService = timeZoneService;
        }

        // GET timezones
        [HttpGet()]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult GetTimeZones()
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
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult GetTimeZonesByCountry(string country)
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
