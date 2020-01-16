using System.Linq;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class ConfigRepository : Repository<Config>, IConfigRepository
    {
        public ConfigRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public int Count(Guid clientId)
        {
            return Context.Configs
                .Where(m => m.ClientId == clientId)
                .Where(m => !m.Deleted)
                .Count();
        }

        public Config Get(Guid configId)
        {
            return Context.Configs
                .Where(m => m.ConfigId == configId)
                .FirstOrDefault();
        }

        public Config GetByName(Guid clientId, string name)
        {
            return Context.Configs
                .Where(m => m.ClientId == clientId)
                .Where(m => m.Name == name)
                .Where(m => !m.Deleted)
                .FirstOrDefault();
        }

        public List<Config> List(Guid clientId)
        {
            return Context.Configs
                .Where(m => m.ClientId == clientId)
                .Where(m => !m.Deleted)
                .OrderBy(m => m.Name)
                .ToList();
        }

        public void LogicalDelete(Guid configId)
        {
            var entity = Context.Configs
                .Where(m => m.ConfigId == configId)
                .FirstOrDefault();

            entity.Deleted = true;
            entity.UpdatedOn = DateTime.UtcNow;
        }
    }
}
