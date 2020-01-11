using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Core.Services
{
    public interface IInvoiceService
    {
        //int Count(Guid clientId);

        PagedList<Invoice> List(Guid clientId, int page, int pageSize);

        Invoice Get(Guid invoiceId);
        Invoice GetByNumber(Guid clientId, int number);

        Invoice Create(Invoice invoice);

        Invoice SetPaid(Guid invoiceId);
    }
}
