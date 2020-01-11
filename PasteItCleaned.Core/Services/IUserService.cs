using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Core.Services
{
    public interface IUserService
    {
        //User Get(Guid userId);
        User GetByCognitoIdAsync(string cognitoId);
        User GetByCognitoUsernameAsync(string cognitoUsername);

        User Create(User user);

        User Update(User userToUpdate, User user);

        void Delete(Guid userId);
    }
}
