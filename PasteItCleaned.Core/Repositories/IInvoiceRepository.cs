using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        int Count(Guid clientId);

        PagedList<Invoice> List(Guid clientId, int page, int pageSize);

        Invoice Get(Guid invoiceId);
        Invoice GetByNumber(Guid clientId, int number);
    }
}
