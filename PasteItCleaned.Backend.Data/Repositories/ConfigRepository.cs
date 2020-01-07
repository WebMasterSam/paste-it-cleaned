using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class ConfigRepository : Repository<Config>, IConfigRepository
    {
        public ConfigRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public async Task<Config> GetByNameAsync(string name)
        {
            return await Context.Configs
                .Where(m => m.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}
