using PasteItCleaned.Core.Models;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByCognitoIdAsync(string cognitoId);
        Task<User> GetByCognitoUsernameAsync(string cognitoUsername);
    }
}
