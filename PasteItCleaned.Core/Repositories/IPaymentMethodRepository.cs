using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethod>
    {
        PaymentMethod GetCurrent(Guid clientId);

        void LogicalDelete(Guid paymentMethodId);
    }
}
