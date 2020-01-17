using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Core.Services
{
    public interface IClientService
    {
        PagedList<Client> List(int page, int pageSize);

        Client Get(Guid clientId);

        Client Create(Client client);

        Client Update(Client client);

        Client DecreaseBalance(Guid clientId, decimal amount);

        void Delete(Guid clientId);
    }
}
