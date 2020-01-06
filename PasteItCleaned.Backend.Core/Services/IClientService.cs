﻿using PasteItCleaned.Backend.Core.Models;
using System;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface IClientService
    {
        Task<Client> GetById(Guid clientId);
        Task<Client> CreateClient(Client client);
        Task UpdateClient(Client clientToBeUpdated, Client client);
        Task DeleteClient(Client client);
    }
}
