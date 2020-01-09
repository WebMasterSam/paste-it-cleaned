using PasteItCleaned.Core.Models;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IApiKeyRepository : IRepository<ApiKey>
    {
        Task<ApiKey> GetByKeyAsync(string key);
    }
}
