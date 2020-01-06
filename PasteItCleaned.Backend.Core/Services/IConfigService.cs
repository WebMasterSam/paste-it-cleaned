using PasteItCleaned.Backend.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface IConfigService
    {
        Task<IEnumerable<Config>> GetAllByClientId(Guid clientId);
        Task<Config> GetById(Guid configId);
        Task<Config> GetByName(string name);
        Task<Config> CreateConfig(Config config);
        Task UpdateConfig(Config configToBeUpdated, Config config);
        Task DeleteConfig(Config config);
    }
}
