using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class HitRepository : Repository<Hit>, IHitRepository
    {
        public HitRepository(PasteItCleanedDbContext context) : base(context)
        { }
    }
}
