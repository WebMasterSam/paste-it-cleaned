using PasteItCleaned.Backend.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllByClientId(Guid clientId);
        Task<Invoice> GetById(Guid invoiceId);
        Task<Invoice> GetByNumber(int number);
        Task<Invoice> CreateInvoice(Invoice invoice);
        Task SetPaid(Guid invoiceId);
    }
}
