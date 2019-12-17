using System;

using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    public class AccountController : ControllerBase
    {
        // GET account/info
        [HttpGet("info")]
        public ActionResult Get()
        {
            return Ok(T.Get("App.Up"));
        }

        // POST account/info
        [HttpPost("info")]
        public ActionResult Post([FromBody] AccountRequest obj)
        {
            return Ok(T.Get("App.Up"));
        }
    }

    public class AccountRequest
    {
        public string any { get; set; }
    }
}
