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

            if (client != null)
                {
                client.Balance -= amount;

                _unitOfWork.Commit();
            }

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

        public Client Update(Client client)
        {
            client.UpdatedOn = DateTime.Now;

            _unitOfWork.Commit();

            return client;
        }
    }
}
