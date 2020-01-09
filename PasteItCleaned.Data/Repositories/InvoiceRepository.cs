using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public async Task<Invoice> GetByNumberAsync(int number)
        {
            return await Context.Invoices
                .Where(m => m.Number == number)
                .FirstOrDefaultAsync();
        }
    }
}
