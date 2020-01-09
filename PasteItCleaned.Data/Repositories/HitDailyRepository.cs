using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class HitDailyRepository : Repository<HitDaily>, IHitDailyRepository
    {
        public HitDailyRepository(PasteItCleanedDbContext context) : base(context)
        { }
    }
}
