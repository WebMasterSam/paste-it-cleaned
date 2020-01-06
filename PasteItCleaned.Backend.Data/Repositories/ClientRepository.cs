using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await Context.Clients
                .ToListAsync();
        }
    }
}
