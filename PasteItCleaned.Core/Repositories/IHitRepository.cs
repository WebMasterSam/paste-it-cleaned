using PasteItCleaned.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IHitRepository : IRepository<Hit>
    {
        Task<Hit> GetByHashAsync(int hash);
    }
}
