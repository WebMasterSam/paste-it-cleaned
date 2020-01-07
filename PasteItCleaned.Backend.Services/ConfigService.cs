using PasteItCleaned.Backend.Core;
using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfigService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Config> CreateConfig(Config config)
        {
            await _unitOfWork.Configs.AddAsync(config);
            await _unitOfWork.CommitAsync();

            return config;
        }

        public async Task DeleteConfig(Config config)
        {
            _unitOfWork.Configs.LogicalDelete(config);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Config>> GetAllByClientId(Guid clientId)
        {
            return await _unitOfWork.Configs.GetAllByParentIdAsync(clientId);
        }

        public async Task<Config> GetById(Guid configId)
        {
            return await _unitOfWork.Configs.GetByIdAsync(configId);
        }

        public async Task<Config> GetByName(string name)
        {
            return await _unitOfWork.Configs.GetByNameAsync(name);
        }

        public async Task UpdateConfig(Config configToBeUpdated, Config config)
        {
            configToBeUpdated.Name = config.Name;
            configToBeUpdated.EmbedExternalImages = config.EmbedExternalImages;
            configToBeUpdated.RemoveClassNames = config.RemoveClassNames;
            configToBeUpdated.RemoveEmptyTags = config.RemoveEmptyTags;
            configToBeUpdated.RemoveIframes = config.RemoveIframes;
            configToBeUpdated.RemoveSpanTags = config.RemoveSpanTags;
            configToBeUpdated.RemoveTagAttributes = config.RemoveTagAttributes;
            configToBeUpdated.UpdatedOn = DateTime.Now;

            await _unitOfWork.CommitAsync();
        }
    }
}
