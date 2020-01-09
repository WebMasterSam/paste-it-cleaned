using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Common.Localization;
using PasteItCleaned.Core.Services;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("plugin")]
    public class PluginController : BaseController
    {
        private readonly IApiKeyService _apiKeyService;
        private readonly IDomainService _domainService;
        private readonly IConfigService _configService;

        public PluginController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IDomainService domainService, IConfigService configService, ILogger<PluginController> logger) : base(apiKeyService, clientService, userService, logger)
        {
            this._apiKeyService = apiKeyService;
            this._domainService = domainService;
            this._configService = configService;
        }

        // GET plugin/api-keys
        [HttpGet("api-keys")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult GetApiKeys()
        {
            Console.WriteLine("PluginController::GetApiKeys");

            return Ok(T.Get("App.Up"));
        }

        // GET plugin/api-keys/{id}
        [HttpGet("api-keys/{id}")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult GetApiKey()
        {
            Console.WriteLine("PluginController::GetApiKey");

            return Ok(T.Get("App.Up"));
        }

        // DELETE plugin/api-keys/{apiKeyid}
        [HttpDelete("api-keys/{apiKeyid}")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult DeleteApiKey(Guid apiKeyid)
        {
            Console.WriteLine("PluginController::DeleteApiKey");

            return Ok(T.Get("App.Up"));
        }

        // DELETE plugin/api-keys/{apiKeyid}/domains/{domainId}
        [HttpDelete("api-keys/{apiKeyid}/domains/{domainId}")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult DeleteApiKeyDomain(Guid apiKeyid, Guid domainId)
        {
            Console.WriteLine("PluginController::DeleteApiKeyDomain");

            return Ok(T.Get("App.Up"));
        }

        // POST api-keys/{apiKeyid}/domains
        [HttpPost("api-keys/{apiKeyid}/domains")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult CreateApiKeDomain([FromBody] PluginConfigRequest obj)
        {
            Console.WriteLine("PluginController::CreateApiKeDomain");

            return Ok(T.Get("App.Up"));
        }

        // GET plugin/config
        [HttpGet("config")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult GetConfig()
        {
            Console.WriteLine("PluginController::GetConfig");

            return Ok(T.Get("App.Up"));
        }

        // PUT plugin/config
        [HttpPut("config")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult UpdateConfig([FromBody] PluginConfigRequest obj)
        {
            Console.WriteLine("PluginController::UpdateConfig");

            return Ok(T.Get("App.Up"));
        }
    }

    public class PluginConfigRequest
    {
        public string any { get; set; }
    }
}
