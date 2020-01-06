using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class ConfigRepository : Repository<Config>, IConfigRepository
    {
        public ConfigRepository(PasteItCleanedDbContext context) : base(context)
        { }
    }
}
