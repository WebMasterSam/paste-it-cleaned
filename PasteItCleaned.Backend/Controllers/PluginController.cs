using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Backend.Entities;
using PasteItCleaned.Common.Localization;
using PasteItCleaned.Core.Helpers;
using PasteItCleaned.Core.Services;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("plugin")]
    public class PluginController : BaseController
    {
        private readonly IApiKeyService _apiKeyService;
        private readonly IDomainService _domainService;
        private readonly IConfigService _configService;

        public PluginController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IDomainService domainService, IConfigService configService, ILogger<PluginController> logger) : base(apiKeyService, clientService, userService, configService, logger)
        {
            this._apiKeyService = apiKeyService;
            this._domainService = domainService;
            this._configService = configService;
        }

        // GET plugin/api-keys
        [HttpGet("api-keys")]
        [ProducesResponseType(typeof(ListApiKeyEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<ListApiKeyEntity> GetApiKeys([FromHeader]string authorization)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var apiKeys = _apiKeyService.List(client.ClientId);

                return Ok(apiKeys);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }

        // POST api-keys
        [HttpPost("api-keys")]
        [ProducesResponseType(typeof(ApiKeyEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<ApiKeyEntity> CreateApiKey([FromHeader]string authorization, [FromBody] PluginConfigRequest obj)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var apiKey = _apiKeyService.Create(new PasteItCleaned.Core.Models.ApiKey { ClientId = client.ClientId, CreatedOn = DateTime.UtcNow, ExpiresOn = DateTime.UtcNow.AddYears(5), Key = ApiKeyHelper.GenerateApiKey() });
                var apiKeyBackend = new ApiKeyEntity();

                apiKeyBackend.ApiKeyId = apiKey.ApiKeyId;
                apiKeyBackend.ExpiresOn = apiKey.ExpiresOn;
                apiKeyBackend.Key = apiKey.Key;

                return Ok(apiKey);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }

        // GET plugin/api-keys/{apiKeyId}
        [HttpGet("api-keys/{apiKeyId}")]
        [ProducesResponseType(typeof(ApiKeyEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<ApiKeyEntity> GetApiKey([FromHeader]string authorization, [FromRoute]Guid apiKeyId)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var apiKey = _apiKeyService.Get(apiKeyId);
                
                if (apiKey == null)
                    return NotFound();

                return Ok(apiKey);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }

        // DELETE plugin/api-keys/{apiKeyId}
        [HttpDelete("api-keys/{apiKeyId}")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult DeleteApiKey([FromHeader]string authorization, [FromRoute]Guid apiKeyId)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var apiKey = _apiKeyService.Get(apiKeyId);

                if (apiKey == null)
                    return NotFound();

                _apiKeyService.Delete(apiKeyId);

                return Ok();
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }

        // DELETE plugin/api-keys/{apiKeyId}/domains/{domainId}
        [HttpDelete("api-keys/{apiKeyId}/domains/{domainId}")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult DeleteApiKeyDomain([FromHeader]string authorization, [FromRoute]Guid apiKeyId, [FromRoute]Guid domainId)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var apiKey = _apiKeyService.Get(apiKeyId);

                if (apiKey == null)
                    return NotFound();

                var domain = _domainService.Get(domainId);

                if (domain == null)
                    return NotFound();

                _domainService.Delete(domainId);

                return Ok();
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }

        // POST api-keys/{apiKeyid}/domains
        [HttpPost("api-keys/{apiKeyid}/domains")]
        [ProducesResponseType(typeof(DomainEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<DomainEntity> CreateApiKeyDomain([FromHeader]string authorization, [FromBody] PluginConfigRequest obj)
        {
            Console.WriteLine("PluginController::CreateApiKeDomain");

            return Ok(T.Get("App.Up"));
        }

        // GET plugin/config
        [HttpGet("config")]
        [ProducesResponseType(typeof(ConfigEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<ConfigEntity> GetConfig([FromHeader]string authorization)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var config = _configService.GetByName(client.ClientId, "DEFAULT");
                
                if (config == null)
                    return NotFound();

                return Ok(config);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }

        // PUT plugin/config
        [HttpPut("config")]
        [ProducesResponseType(typeof(ConfigEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<ConfigEntity> UpdateConfig([FromHeader]string authorization, [FromBody] PluginConfigRequest request)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var config = _configService.Get(request.config.ConfigId);

                if (config == null)
                    return NotFound();

                config.EmbedExternalImages = request.config.EmbedExternalImages;
                config.RemoveClassNames = request.config.RemoveClassNames;
                config.RemoveEmptyTags = request.config.RemoveEmptyTags;
                config.RemoveIframes = request.config.RemoveIframes;
                config.RemoveSpanTags = request.config.RemoveSpanTags;
                config.RemoveTagAttributes = request.config.RemoveTagAttributes;
                config.UpdatedOn = DateTime.UtcNow;

                _configService.Update(config);

                return Ok(config);
            }
            catch (Exception ex)
            {
                Log.LogError("Exception", ex);
                return BadRequest("A technical error has occured when calling this API method.");
            }
        }
    }

    public class PluginConfigRequest
    {
        public ConfigEntity config { get; set; }
    }
}
