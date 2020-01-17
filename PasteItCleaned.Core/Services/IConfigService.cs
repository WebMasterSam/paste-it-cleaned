using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Core.Services
{
    public interface IConfigService
    {
        List<Config> List(Guid clientId);

        Config Get(Guid configId);
        Config GetByName(Guid clientId, string name);

        Config Create(Config config);

        Config Update(Config config);

        void Delete(Guid configId);
    }
}
