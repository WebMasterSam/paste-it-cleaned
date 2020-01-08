using System;
using Microsoft.AspNetCore.Mvc;
using PasteItCleaned.Backend.Core.Services;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("timezones")]
    public class TimeZoneController : BaseController
    {
        private ITimeZoneService _timeZoneService = null;

        public TimeZoneController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, ITimeZoneService timeZoneService) : base(apiKeyService, clientService, userService)
        {
            this._timeZoneService = timeZoneService;
        }

        // GET timezones
        [HttpGet()]
        public ActionResult GetAll()
        {
            Console.WriteLine("TimeZoneController::GetAll");

            var timeZones = _timeZoneService.GetAll();

            return Ok(timeZones);
        }

        // GET timezones/{country}/
        [HttpGet("{country}")]
        public ActionResult GetAllByCountry(string country)
        {
            Console.WriteLine("TimeZoneController::GetAllByCountry");

            var timeZones = _timeZoneService.GetAll(country);

            return Ok(timeZones);
        }
    }
}
