using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Core.Services
{
    public interface IPaymentMethodService
    {
        PaymentMethod GetCurrent(Guid clientId);

        PaymentMethod Create(PaymentMethod paymentMethod);

        //PaymentMethod Update(PaymentMethod paymentMethodToUpdate, PaymentMethod paymentMethod);

        void Delete(Guid paymentMethodId);
        void DeleteCurrent(Guid clientId);
    }
}
