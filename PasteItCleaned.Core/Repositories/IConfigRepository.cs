using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IConfigRepository : IRepository<Config>
    {
        int Count(Guid clientId);
        List<Config> List(Guid clientId);

        Config Get(Guid configId);
        Config GetByName(string name);

        void LogicalDelete(Guid configId);
    }
}
