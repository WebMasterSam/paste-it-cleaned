using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Client> CreateClient(Client client)
        {
            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.CommitAsync();

            return client;
        }

        public async Task DeleteClient(Client client)
        {
            _unitOfWork.Clients.LogicalDelete(client);

            await _unitOfWork.CommitAsync();
        }

        public async Task<Client> GetById(Guid clientId)
        {
            return await _unitOfWork.Clients.GetByIdAsync(clientId);
        }

        public async Task UpdateClient(Client clientToBeUpdated, Client client)
        {
            clientToBeUpdated.Address = client.Address;
            clientToBeUpdated.Balance = client.Balance;
            clientToBeUpdated.BusinessName = client.BusinessName;
            clientToBeUpdated.City = client.City;
            clientToBeUpdated.Country = client.Country;
            clientToBeUpdated.FirstName = client.FirstName;
            clientToBeUpdated.LastName = client.LastName;
            clientToBeUpdated.PhoneNumber = client.PhoneNumber;
            clientToBeUpdated.State = client.State;
            clientToBeUpdated.UpdatedOn = DateTime.Now;

            await _unitOfWork.CommitAsync();
        }
    }
}
