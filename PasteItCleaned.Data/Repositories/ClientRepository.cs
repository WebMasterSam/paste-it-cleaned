using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System.Linq;
using System;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public int Count()
        {
            return Context.Clients
                .Where(m => !m.Deleted)
                .Count();
        }

        public Client Get(Guid clientId)
        {
            return Context.Clients
                .Where(m => m.ClientId == clientId)
                .FirstOrDefault();
        }

        public PagedList<Client> List(int page, int pageSize)
        {
            var query = Context.Clients
                .Where(m => !m.Deleted)
                .OrderByDescending(m => m.CreatedOn);

            return this.PagedList(query, page, pageSize);
        }

        public void LogicalDelete(Guid clientId)
        {
            var entity = Context.Clients
                .Where(m => m.ClientId == clientId)
                .FirstOrDefault();

            entity.Deleted = true;
            entity.UpdatedOn = DateTime.UtcNow;
        }
    }
}
