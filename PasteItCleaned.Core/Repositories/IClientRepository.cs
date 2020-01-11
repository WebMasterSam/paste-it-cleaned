using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        int Count();
        PagedList<Client> List(int page, int pageSize);

        Client Get(Guid clientId);

        void LogicalDelete(Guid clientId);
    }
}
