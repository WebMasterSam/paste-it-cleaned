using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class DomainRepository : Repository<Domain>, IDomainRepository
    {
        public DomainRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public async Task<Domain> GetByNameAsync(string name)
        {
            return await Context.Domains
                .Where(m => m.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}
