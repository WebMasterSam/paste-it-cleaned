using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<User> CreateUser(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return user;
        }

        public async Task DeleteUser(User user)
        {
            _unitOfWork.Users.LogicalDelete(user);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<User>> GetAllByClientId(Guid clientId)
        {
            return await _unitOfWork.Users.GetAllByParentIdAsync(clientId);
        }

        public async Task<User> GetById(Guid userId)
        {
            return await _unitOfWork.Users.GetByIdAsync(userId);
        }

        public async Task<User> GetByCognitoId(string key)
        {
            return await _unitOfWork.Users.GetByCognitoIdAsync(key);
        }

        public async Task<User> GetByCognitoUsername(string key)
        {
            return await _unitOfWork.Users.GetByCognitoUsernameAsync(key);
        }

        public async Task UpdateUser(User userToBeUpdated, User user)
        {
            userToBeUpdated.UpdatedOn = DateTime.Now;

            await _unitOfWork.CommitAsync();
        }
    }
}
