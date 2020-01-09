using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Core.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllByClientId(Guid clientId);
        Task<User> GetById(Guid userId);
        Task<User> GetByCognitoId(string cognitoId);
        Task<User> GetByCognitoUsername(string cognitoUsername);
        Task<User> CreateUser(User user);
        Task UpdateUser(User userToBeUpdated, User user);
        Task DeleteUser(User user);
    }
}
