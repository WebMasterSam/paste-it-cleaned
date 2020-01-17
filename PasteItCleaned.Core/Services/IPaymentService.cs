using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Core.Services
{
    public interface IPaymentService
    {
        Payment Get(Guid paymentId);
        Payment GetByInvoice(Guid invoiceId);

        Payment Create(Payment payment);

        Payment Update(Payment payment);
    }
}
