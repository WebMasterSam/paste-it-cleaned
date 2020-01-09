using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.CommitAsync();

            return invoice;
        }

        public async Task DeleteInvoice(Invoice apiKey)
        {
            _unitOfWork.Invoices.LogicalDelete(apiKey);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Invoice>> GetAllByClientIdAsync(Guid clientId)
        {
            return await _unitOfWork.Invoices.GetAllByParentIdAsync(clientId);
        }

        public async Task<Invoice> GetByIdAsync(Guid apiKeyId)
        {
            return await _unitOfWork.Invoices.GetByIdAsync(apiKeyId);
        }

        public async Task<Invoice> GetByNumberAsync(int number)
        {
            return await _unitOfWork.Invoices.GetByNumberAsync(number);
        }

        public async Task SetPaidAsync(Guid invoiceId)
        {
            var invoice = await GetByIdAsync(invoiceId);

            invoice.Paid = true;
            invoice.PaidOn = DateTime.Now;

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateApiKey(Invoice invoiceToBeUpdated, Invoice invoice)
        {
            invoiceToBeUpdated.Paid = invoice.Paid;

            await _unitOfWork.CommitAsync();
        }
    }
}
