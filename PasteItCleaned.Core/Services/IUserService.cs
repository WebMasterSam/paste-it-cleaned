using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Core.Services
{
    public interface IUserService
    {
        //User Get(Guid userId);
        User GetByCognitoId(string cognitoId);
        User GetByCognitoUsername(string cognitoUsername);

        User Create(User user);

        User Update(User user);
    }
}
