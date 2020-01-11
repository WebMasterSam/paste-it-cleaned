using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;

namespace PasteItCleaned.Backend.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Invoice Create(Invoice invoice)
        {
            _unitOfWork.Invoices.Add(invoice);
            _unitOfWork.Commit();

            return invoice;
        }

        public Invoice Get(Guid invoiceId)
        {
            return _unitOfWork.Invoices.Get(invoiceId);
        }

        public Invoice GetByNumber(Guid clientId, int number)
        {
            return _unitOfWork.Invoices.GetByNumber(clientId, number);
        }

        public PagedList<Invoice> List(Guid clientId, int page, int pageSize)
        {
            return _unitOfWork.Invoices.List(clientId, page, pageSize);
        }

        public Invoice SetPaid(Guid invoiceId)
        {
            var invoice = _unitOfWork.Invoices.Get(invoiceId);

            invoice.Paid = invoice.Paid;

            _unitOfWork.Commit();

            return invoice;
        }
    }
}
