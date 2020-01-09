using PasteItCleaned.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IConfigRepository : IRepository<Config>
    {
        Task<Config> GetByNameAsync(string name);
    }
}
