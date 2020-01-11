using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByCognitoId(string cognitoId);
        User GetByCognitoUsername(string cognitoUsername);
    }
}
