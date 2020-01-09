using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public async Task<User> GetByCognitoIdAsync(string cognitoId)
        {
            return await Context.Users
                .Where(m => m.CognitoId == cognitoId)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByCognitoUsernameAsync(string cognitoUsername)
        {
            return await Context.Users
                .Where(m => m.CognitoUsername == cognitoUsername)
                .FirstOrDefaultAsync();
        }
    }
}
