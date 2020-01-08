using System;

using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("plugin")]
    public class PluginController : ControllerBase
    {
        // GET plugin/api-keys
        [HttpGet("api-keys")]
        public ActionResult GetApiKeys()
        {
            Console.WriteLine("PluginController::GetApiKeys");

            return Ok(T.Get("App.Up"));
        }

        // GET plugin/api-keys/{id}
        [HttpGet("api-keys/{id}")]
        public ActionResult GetApiKey()
        {
            Console.WriteLine("PluginController::GetApiKey");

            return Ok(T.Get("App.Up"));
        }

        // DELETE plugin/api-keys/{id}
        [HttpDelete("api-keys/{id}")]
        public ActionResult DeleteApiKey()
        {
            Console.WriteLine("PluginController::DeleteApiKey");

            return Ok(T.Get("App.Up"));
        }

        // GET plugin/config
        [HttpGet("config")]
        public ActionResult GetConfig()
        {
            Console.WriteLine("PluginController::GetConfig");

            return Ok(T.Get("App.Up"));
        }

        // POST plugin/config
        [HttpPost("config")]
        public ActionResult Post([FromBody] PluginConfigRequest obj)
        {
            Console.WriteLine("PluginController::Post");

            return Ok(T.Get("App.Up"));
        }
    }

    public class PluginConfigRequest
    {
        public string any { get; set; }
    }
}
