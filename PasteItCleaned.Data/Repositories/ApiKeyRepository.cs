using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class ApiKeyRepository : Repository<ApiKey>, IApiKeyRepository
    {
        public ApiKeyRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public async Task<ApiKey> GetByKeyAsync(string key)
        {
            return await Context.ApiKeys
                .Where(m => m.Key == key)
                .FirstOrDefaultAsync();
        }
    }
}
