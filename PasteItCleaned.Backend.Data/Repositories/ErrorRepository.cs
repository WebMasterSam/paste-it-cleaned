using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class ErrorRepository : Repository<Error>, IErrorRepository
    {
        public ErrorRepository(PasteItCleanedDbContext context) : base(context)
        { }
    }
}
