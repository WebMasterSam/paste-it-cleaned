using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Backend.Entities;
using PasteItCleaned.Core.Helpers;
using PasteItCleaned.Core.Models;
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
                var apiKeysEntity = new ListApiKeyEntity();

                foreach (var apiKey in apiKeys)
                {
                    var apiKeyEntity = new ApiKeyEntity { ApiKeyId = apiKey.ApiKeyId, ExpiresOn = apiKey.ExpiresOn, Key = apiKey.Key, Domains = new List<DomainEntity>() };
                    var domains = _domainService.List(apiKey.ApiKeyId);

                    foreach (Domain domain in domains)
                    {
                        apiKeyEntity.Domains.Add(new DomainEntity { DomainId = domain.DomainId, Name = domain.Name });
                    }

                    apiKeysEntity.Add(apiKeyEntity);
                }

                return Ok(apiKeysEntity);
            }
            catch (Exception ex)
            {
                return this.LogAndReturn500(ex);
            }
        }

        // POST api-keys
        [HttpPost("api-keys")]
        [ProducesResponseType(typeof(ApiKeyEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<ApiKeyEntity> CreateApiKey([FromHeader]string authorization)
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
                return this.LogAndReturn500(ex);
            }
        }

        // PUT api-keys/{apiKeyId}
        [HttpPut("api-keys/{apiKeyId}")]
        [ProducesResponseType(typeof(ApiKeyEntity), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<ApiKeyEntity> UpdateApiKey([FromHeader]string authorization, [FromBody] PluginApiKeyRequest req)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            if (req == null)
                return StatusCode(400);

            if (req.apiKey == null)
                return StatusCode(400);

            try
            {
                var apiKey = _apiKeyService.Get(req.apiKey.ApiKeyId);
                var domains = _domainService.List(req.apiKey.ApiKeyId);

                if (apiKey == null)
                    return StatusCode(404);

                apiKey.ExpiresOn = req.apiKey.ExpiresOn;
                apiKey.UpdatedOn = DateTime.UtcNow;

                _apiKeyService.Update(apiKey);

                var apiKeyEntity = new ApiKeyEntity();

                apiKeyEntity.ApiKeyId = apiKey.ApiKeyId;
                apiKeyEntity.ExpiresOn = apiKey.ExpiresOn;
                apiKeyEntity.Key = apiKey.Key;
                apiKeyEntity.Domains = new List<DomainEntity>();

                foreach (Domain domain in domains)
                {
                    apiKeyEntity.Domains.Add(new DomainEntity { DomainId = domain.DomainId, Name = domain.Name });
                }

                return Ok(apiKey);
            }
            catch (Exception ex)
            {
                return this.LogAndReturn500(ex);
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
                var domains = _domainService.List(apiKeyId);

                if (apiKey == null)
                    return NotFound();

                var apiKeyEntity = new ApiKeyEntity();

                apiKeyEntity.ApiKeyId = apiKey.ApiKeyId;
                apiKeyEntity.ExpiresOn = apiKey.ExpiresOn;
                apiKeyEntity.Key = apiKey.Key;

                foreach (Domain domain in domains)
                {
                    apiKeyEntity.Domains.Add(new DomainEntity { DomainId = domain.DomainId, Name = domain.Name });
                }

                return Ok(apiKeyEntity);
            }
            catch (Exception ex)
            {
                return this.LogAndReturn500(ex);
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
                return this.LogAndReturn500(ex);
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
                return this.LogAndReturn500(ex);
            }
        }

        // POST api-keys/{apiKeyid}/domains
        [HttpPost("api-keys/{apiKeyid}/domains")]
        [ProducesResponseType(typeof(DomainEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<DomainEntity> CreateApiKeyDomain([FromHeader]string authorization, [FromRoute]Guid apiKeyId, [FromBody] PluginApiKeyDomainRequest req)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var apiKey = _apiKeyService.Get(apiKeyId);
                var domain = _domainService.GetByName(apiKeyId, req.domainName);

                if (apiKey == null)
                    return NotFound();

                if (domain == null) {
                    domain = _domainService.Create(new Domain { ApiKeyId = apiKeyId, CreatedOn = DateTime.UtcNow, Name = req.domainName });
                }

                var domainEntity = new DomainEntity();

                domainEntity.DomainId = domain.DomainId;
                domainEntity.Name = domain.Name;

                return Ok(domainEntity);
            }
            catch (Exception ex)
            {
                return this.LogAndReturn500(ex);
            }
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
                return this.LogAndReturn500(ex);
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
                return this.LogAndReturn500(ex);
            }
        }
    }

    public class PluginConfigRequest
    {
        public ConfigEntity config { get; set; }
    }

    public class PluginApiKeyRequest
    {
        public ApiKeyEntity apiKey { get; set; }
    }

    public class PluginApiKeyDomainRequest
    {
        public string domainName { get; set; }
    }
}
