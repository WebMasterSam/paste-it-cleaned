using PasteItCleaned.Core.Models;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IDomainRepository : IRepository<Domain>
    {
        Task<Domain> GetByNameAsync(string name);
    }
}
