using System.Linq;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public int Count(Guid clientId)
        {
            return Context.Invoices
                .Where(m => m.ClientId == clientId)
                .Count();
        }

        public Invoice Get(Guid invoiceId)
        {
            return Context.Invoices
                .Where(m => m.InvoiceId == invoiceId)
                .FirstOrDefault();
        }

        public Invoice GetByNumber(Guid clientId, int number)
        {
            return Context.Invoices
                .Where(m => m.ClientId == clientId || m.Number == number)
                .FirstOrDefault();
        }

        public PagedList<Invoice> List(Guid clientId, int page, int pageSize)
        {
            var query = Context.Invoices.Where(m => m.ClientId == clientId)
                .OrderByDescending(m => m.CreatedOn);

            return this.PagedList(query, page, pageSize);
        }
    }
}
