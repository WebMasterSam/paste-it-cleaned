using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class HitRepository : Repository<Hit>, IHitRepository
    {
        public HitRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public async Task<Hit> GetByHashAsync(int hash)
        {
            return await Context.Hits
                .Where(m => m.Hash == hash)
                .FirstOrDefaultAsync();
        }
    }
}
