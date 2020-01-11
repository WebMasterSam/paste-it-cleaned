using System.Linq;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public User GetByCognitoId(string cognitoId)
        {
            return Context.Users
                .Where(m => m.CognitoId == cognitoId)
                .Where(m => !m.Deleted)
                .FirstOrDefault();
        }

        public User GetByCognitoUsername(string cognitoUsername)
        {
            return Context.Users
                .Where(m => m.CognitoUsername == cognitoUsername)
                .Where(m => !m.Deleted)
                .FirstOrDefault();
        }
    }
}
