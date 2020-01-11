using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;

namespace PasteItCleaned.Backend.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Client Create(Client client)
        {
            _unitOfWork.Clients.Add(client);
            _unitOfWork.Commit();

            return client;
        }

        public Client DecreaseBalance(Guid clientId, decimal amount)
        {
            var client = _unitOfWork.Clients.Get(clientId);

            client.Balance -= amount;

            _unitOfWork.Commit();

            return client;
        }

        public void Delete(Guid clientId)
        {
            _unitOfWork.Clients.LogicalDelete(clientId);

            _unitOfWork.Commit();
        }

        public Client Get(Guid clientId)
        {
            return _unitOfWork.Clients.Get(clientId);
        }

        public PagedList<Client> List(int page, int pageSize)
        {
            return _unitOfWork.Clients.List(page, pageSize);
        }

        public Client Update(Client clientToUpdate, Client client)
        {
            clientToUpdate.Address = client.Address;
            clientToUpdate.Balance = client.Balance;
            clientToUpdate.BusinessName = client.BusinessName;
            clientToUpdate.City = client.City;
            clientToUpdate.Country = client.Country;
            clientToUpdate.FirstName = client.FirstName;
            clientToUpdate.LastName = client.LastName;
            clientToUpdate.PhoneNumber = client.PhoneNumber;
            clientToUpdate.State = client.State;
            clientToUpdate.UpdatedOn = DateTime.Now;

            _unitOfWork.Commit();

            return clientToUpdate;
        }
    }
}
