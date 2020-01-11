﻿using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfigService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Config Create(Config config)
        {
            _unitOfWork.Configs.Add(config);
            _unitOfWork.Commit();

            return config;
        }

        public void Delete(Guid configId)
        {
            _unitOfWork.Configs.LogicalDelete(configId);

            _unitOfWork.Commit();
        }

        public Config Get(Guid configId)
        {
            return _unitOfWork.Configs.Get(configId);
        }

        public Config GetByName(Guid clientId, string name)
        {
            return _unitOfWork.Configs.GetByName(clientId, name);
        }

        public List<Config> List(Guid clientId)
        {
            return _unitOfWork.Configs.List(clientId);
        }

        public Config Update(Config configToUpdate, Config config)
        {
            configToUpdate.Name = config.Name;
            configToUpdate.EmbedExternalImages = config.EmbedExternalImages;
            configToUpdate.RemoveClassNames = config.RemoveClassNames;
            configToUpdate.RemoveEmptyTags = config.RemoveEmptyTags;
            configToUpdate.RemoveIframes = config.RemoveIframes;
            configToUpdate.RemoveSpanTags = config.RemoveSpanTags;
            configToUpdate.RemoveTagAttributes = config.RemoveTagAttributes;
            configToUpdate.UpdatedOn = DateTime.Now;

            _unitOfWork.Commit();

            return configToUpdate;
        }
    }
}
