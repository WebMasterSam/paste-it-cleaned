using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Payment Get(Guid paymentId);
        Payment GetByInvoice(Guid invoiceId);
    }
}
