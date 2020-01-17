using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;

namespace PasteItCleaned.Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public User GetByCognitoId(string cognitoId)
        {
            return _unitOfWork.Users.GetByCognitoId(cognitoId);
        }

        public User GetByCognitoUsername(string cognitoUsername)
        {
            return _unitOfWork.Users.GetByCognitoUsername(cognitoUsername);
        }

        public User Create(User user)
        {
            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            return user;
        }

        public User Update(User user)
        {
            user.UpdatedOn = DateTime.Now;

            _unitOfWork.Commit();

            return user;
        }
    }
}
