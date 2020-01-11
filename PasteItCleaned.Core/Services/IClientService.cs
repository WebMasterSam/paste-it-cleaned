using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Core.Services
{
    public interface IClientService
    {
        PagedList<Client> List(Guid clientId);

        Client Get(Guid clientId);

        Client Create(Client client);

        Client Update(Client clientToUpdate, Client client);

        void Delete(Client client);
    }
}
