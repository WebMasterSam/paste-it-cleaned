using PasteItCleaned.Backend.Core.Models;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IDomainRepository : IRepository<ApiKey>
    {
        Task<ApiKey> GetByNameAsync(string name);
    }
}
