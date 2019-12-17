using System;

using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    public class PluginController : ControllerBase
    {
        // GET plugin/api-keys/{id}
        [HttpGet("api-keys/{id}")]
        public ActionResult GetApiKey()
        {
            return Ok(T.Get("App.Up"));
        }

        // GET plugin/api-keys
        [HttpGet("api-keys")]
        public ActionResult GetApiKeys()
        {
            return Ok(T.Get("App.Up"));
        }

        // GET plugin/config
        [HttpGet("config")]
        public ActionResult GetConfig()
        {
            return Ok(T.Get("App.Up"));
        }

        // POST plugin/config
        [HttpPost("config")]
        public ActionResult Post([FromBody] PluginConfigRequest obj)
        {
            return Ok(T.Get("App.Up"));
        }
    }

    public class PluginConfigRequest
    {
        public string any { get; set; }
    }
}
