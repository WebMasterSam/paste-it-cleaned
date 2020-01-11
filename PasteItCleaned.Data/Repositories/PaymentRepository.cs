using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System;
using System.Linq;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public Payment Get(Guid paymentId)
        {
            return Context.Payments
                .Where(m => m.PaymentId == paymentId)
                .FirstOrDefault();
        }

        public Payment GetByInvoice(Guid invoiceId)
        {
            return Context.Payments
                .Where(m => m.InvoiceId == invoiceId)
                .FirstOrDefault();
        }
    }
}
