using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Core.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllByClientIdAsync(Guid clientId);
        Task<Invoice> GetByIdAsync(Guid invoiceId);
        Task<Invoice> GetByNumberAsync(int number);
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        Task SetPaidAsync(Guid invoiceId);
    }
}
